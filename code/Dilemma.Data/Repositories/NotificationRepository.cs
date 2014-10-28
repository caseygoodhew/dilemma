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
                var notifications =
                    context.Notifications.Include(x => x.Answer)
                        .Include(x => x.Question)
                        .Include(x => x.Moderation)
                        .Where(x => x.ForUser.UserId == forUserId)
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

                return ConverterFactory.ConvertMany<Notification, T>(notifications);
            }
        }

        /// <summary>
        /// Raises a new notification for a user.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="forUserId">The <see cref="User"/> that the notification should be created for.</param>
        /// <param name="notificationType">The <see cref="NotificationType"/>.</param>
        /// <param name="id">The id of the object that the notification refers to.</param>
        public void Raise(DilemmaContext context, int forUserId, NotificationType notificationType, int id)
        {
            var notification = new Notification
                                   {
                                       ForUser = new User
                                                     {
                                                         UserId = forUserId
                                                     },
                                       NotificationType = notificationType,
                                       CreatedDateTime = TimeSource.Value.Now
                                   };

            context.EnsureAttached(notification.ForUser, x => x.UserId);

            switch (notificationType)
            {
                case NotificationType.QuestionAnswered:
                    var questionId = context.Answers.Where(x => x.AnswerId == id).Select(
                        x => new
                                 {
                                     x.Question.QuestionId
                                 }).Single().QuestionId;

                    notification.Answer = context.GetOrAttachNew<Answer, int>(id, x => x.AnswerId);
                    notification.Question = context.GetOrAttachNew<Question, int>(questionId, x => x.QuestionId);
                    break;

                case NotificationType.PostRejected:
                    notification.Moderation = context.GetOrAttachNew<Moderation, int>(id, x => x.ModerationId);
                    break;

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

        private static List<Notification> FindNotifications(
            DilemmaContext context,
            int? forUserId,
            NotificationLookupBy notificationLookupBy,
            int id)
        {
            return
                context.Notifications.Where(
                    x =>
                    (notificationLookupBy == NotificationLookupBy.QuestionId
                     && x.NotificationType == NotificationType.QuestionAnswered && x.Answer.Question.QuestionId == id)
                    || (notificationLookupBy == NotificationLookupBy.ModerationId
                        && x.NotificationType == NotificationType.PostRejected && x.Moderation.ModerationId == id))
                    .Where(x => x.ActionedDateTime == null)
                    .Where(x => x.ForUser.UserId == (forUserId ?? x.ForUser.UserId))
                    .ToList();
        }
    }
}
