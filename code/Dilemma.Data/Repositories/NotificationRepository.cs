using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Notification repository implementation.
    /// </summary>
    internal class NotificationRepository : IInternalNotificationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        public int CountNewNotifications(int forUserId)
        {
            using (var context = new DilemmaContext())
            {
                return context.Notifications.Where(x => x.ForUser.UserId == forUserId).Count(x => x.ActionedDateTime == null);
            }
        }

        public IEnumerable<T> GetTopUnread<T>(int forUserId, int maxGroupings) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var notifications =
                    QueryNotifications(
                        context.Notifications
                            .Where(x => x.ForUser.UserId == forUserId)
                            .Where(x => !x.ActionedDateTime.HasValue)
                            .GroupBy(x => x.Question == null ? x.NotificationId : x.Question.QuestionId)
                            .OrderByDescending(g => g.Max(x => x.CreatedDateTime))
                            .Take(maxGroupings)
                            .SelectMany(x => x));

                return ConverterFactory.ConvertMany<Notification, T>(notifications);
            }
        }

        /// <summary>
        /// Gets all notifications for a given user.
        /// </summary>
        /// <typeparam name="T">The type to convert the output <see cref="Notification"/>s to.</typeparam>
        /// <param name="forUserId">The userId to get the notifications for.</param>
        /// <returns>The converted <see cref="Notification"/>s.</returns>
        public IEnumerable<T> GetAll<T>(int forUserId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var notifications = QueryNotifications(context.Notifications.Where(x => x.ForUser.UserId == forUserId));
                
                return ConverterFactory.ConvertMany<Notification, T>(notifications);
            }
        }

        /// <summary>
        /// Raises a new notification for a user.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="forUserId">The <see cref="User"/> that the notification should be created for.</param>
        /// <param name="notificationType">The <see cref="NotificationType"/>.</param>
        /// <param name="notificationTarget">The <see cref="NotificationTarget"/>.</param>
        /// <param name="id">The id of the object that the notification refers to.</param>
        public void Raise(DilemmaContext context, int forUserId, NotificationType notificationType, NotificationTarget notificationTarget, int id)
        {
            var forUser = context.GetOrAttachNew<User, int>(forUserId, x => x.UserId);
            
            var notification = new Notification
                                   {
                                       ForUser = forUser,
                                       NotificationType = notificationType,
                                       NotificationTarget = notificationTarget,
                                       CreatedDateTime = TimeSource.Value.Now
                                   };

            int questionId;
            
            switch (notificationType)
            {
                case NotificationType.QuestionApproved:
                case NotificationType.QuestionRejected:
                case NotificationType.OpenForVoting:
                    
                    var question = context.Questions.Where(x => x.QuestionId == id).Select(x => new { x.QuestionId }).Single();
                    notification.Question = context.GetOrAttachNew<Question, int>(question.QuestionId, x => x.QuestionId);
                    break;

                case NotificationType.AnswerApproved:
                case NotificationType.AnswerRejected:
                case NotificationType.VoteOnAnswer:
                case NotificationType.BestAnswerAwarded:

                    var answer = context.Answers.Where(x => x.AnswerId == id).Select(x => new { x.Question.QuestionId, x.AnswerId }).Single();
                    notification.Question = context.GetOrAttachNew<Question, int>(answer.QuestionId, x => x.QuestionId);
                    notification.Answer = context.GetOrAttachNew<Answer, int>(answer.AnswerId, x => x.AnswerId);
                    break;

                case NotificationType.FollowupApproved:
                case NotificationType.FollowupRejected:

                    var followup = context.Followups.Where(x => x.FollowupId == id).Select(x => new { x.Question.QuestionId, x.FollowupId }).Single();
                    notification.Question = context.GetOrAttachNew<Question, int>(followup.QuestionId, x => x.QuestionId);
                    notification.Followup = context.GetOrAttachNew<Followup, int>(followup.FollowupId, x => x.FollowupId);
                    break;

                case NotificationType.FlaggedQuestionApproved:
                case NotificationType.FlaggedAnswerApproved:
                case NotificationType.FlaggedFollowupApproved:
                case NotificationType.FlaggedQuestionRejected:
                case NotificationType.FlaggedAnswerRejected:
                case NotificationType.FlaggedFollowupRejected:
                    return;

                default:
                    throw new ArgumentOutOfRangeException("notificationType");
            }

            context.Notifications.Add(notification);

            context.SaveChangesVerbose();
        }

        /// <summary>
        /// Mutes a notification for a given user.
        /// </summary>
        /// <param name="forUserId">The <see cref="User"/> id that the <see cref="Notification"/> belongs to.</param>
        /// <param name="notificationLookupBy">The <see cref="NotificationLookupBy"/>.</param>
        /// <param name="id">The id to use to lookup the notification.</param>
        public void Mute(int forUserId, NotificationLookupBy notificationLookupBy, int id)
        {
            using (var context = new DilemmaContext())
            {
                var notifications = FindNotifications(context, forUserId, notificationLookupBy, id);

                var now = TimeSource.Value.Now;
                notifications.ForEach(
                    x =>
                        {
                            x.ActionedDateTime = now;
                            context.Entry(x).State = EntityState.Modified;
                        });

                context.SaveChangesVerbose();
            }
        }

        /// <summary>
        /// Deletes all existing notifications that refer to the provided <see cref="id"/>.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="notificationLookupBy">The <see cref="NotificationLookupBy"/>.</param>
        /// <param name="id">The id of the object that can be used to find the notification.</param>
        public void Delete(DilemmaContext context, NotificationLookupBy notificationLookupBy, int id)
        {
            var notifications = FindNotifications(context, null, notificationLookupBy, id);

            notifications.ForEach(x => context.Entry(x).State = EntityState.Deleted);

            context.SaveChangesVerbose();
        }

        private static List<Notification> FindNotifications(DilemmaContext context, int? forUserId, NotificationLookupBy notificationLookupBy, int id)
        {
            var query =
                context.Notifications.Where(x => x.ActionedDateTime == null)
                    .Where(x => x.ForUser.UserId == (forUserId ?? x.ForUser.UserId));

            switch (notificationLookupBy)
            {
                case NotificationLookupBy.Question:
                    return query.Where(x => x.Question != null).Where(x => x.Question.QuestionId == id).ToList();

                case NotificationLookupBy.Answer:
                    return query.Where(x => x.Answer != null).Where(x => x.Answer.AnswerId == id).ToList();

                case NotificationLookupBy.Followup:
                    return query.Where(x => x.Followup != null).Where(x => x.Followup.FollowupId == id).ToList();
                
                case NotificationLookupBy.Moderation:
                    return query.Where(x => x.Moderation != null).Where(x => x.Moderation.ModerationId == id).ToList();
                    
                default:
                    throw new ArgumentOutOfRangeException("notificationLookupBy");
            }
        }

        private static IList<Notification> QueryNotifications(IQueryable<Notification> query)
        {
            return query.Include(x => x.Answer)
                        .Include(x => x.Question)
                        .Include(x => x.Moderation)
                        .Select(
                            x => new
                            {
                                x.ActionedDateTime,
                                x.CreatedDateTime,
                                x.NotificationType,
                                AnswerId = x.Answer != null ? (int?)x.Answer.AnswerId : null,
                                x.Question,
                                ModerationId = x.Moderation != null ? (int?)x.Moderation.ModerationId : null
                            }).OrderByDescending(x => x.CreatedDateTime).ToList().Select(
                                         x =>
                                         {
                                             var answer = x.AnswerId.HasValue
                                                              ? new Answer
                                                              {
                                                                  AnswerId = x.AnswerId.Value,
                                                                  Question = x.Question
                                                              }
                                                              : null;

                                             var moderation = x.ModerationId.HasValue
                                                                  ? new Moderation
                                                                  {
                                                                      ModerationId = x.ModerationId.Value,
                                                                      Question = x.Question
                                                                  }
                                                                  : null;

                                             return new Notification
                                             {
                                                 ActionedDateTime = x.ActionedDateTime,
                                                 CreatedDateTime = x.CreatedDateTime,
                                                 NotificationType = x.NotificationType,
                                                 Answer = answer,
                                                 Question = x.Question,
                                                 Moderation = moderation
                                             };
                                         }).ToList();
        }
    }
}
