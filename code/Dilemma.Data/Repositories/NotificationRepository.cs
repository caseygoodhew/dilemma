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
    internal class NotificationRepository : INotificationRepository, IInternalNotificationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        public IEnumerable<T> GetAll<T>(int forUserId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var notifications = context.Notifications
                    .Include(x => x.Answer)
                    //.Include(x => x.Answer.Question)
                    .Include(x => x.Question)
                    .Include(x => x.Moderation)
                    //.Include(x => x.Moderation.Question)
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

        public void Raise(DilemmaContext context, int forUserId, NotificationType notificationType, int id)
        {
            var notification = new Notification
                    {
                        ForUser = new User { UserId = forUserId },
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
                    
                    notification.Answer = context.EnsureAttached(new Answer { AnswerId = id }, x => x.AnswerId);
                    notification.Question = context.EnsureAttached(new Question { QuestionId = questionId }, x => x.QuestionId);
                    break;
                        
                case NotificationType.PostRejected:
                    notification.Moderation = context.EnsureAttached(new Moderation { ModerationId = id }, x => x.ModerationId);
                    break;
                        
                default:
                    throw new ArgumentOutOfRangeException("notificationType");
            }

            context.Notifications.Add(notification);
                
            context.SaveChangesVerbose();
        }

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

        public void Delete(DilemmaContext context, NotificationLookupBy notificationLookupBy, int id)
        {
            var notifications = FindNotifications(context, null, notificationLookupBy, id);

            notifications.ForEach(x => context.Entry(x).State = EntityState.Deleted);

            context.SaveChangesVerbose();
        }

        private List<Notification> FindNotifications(DilemmaContext context, int? forUserId, NotificationLookupBy notificationLookupBy, int id)
        {
            return context.Notifications
                        .Where(
                            x =>
                            (
                                notificationLookupBy == NotificationLookupBy.QuestionId
                                && x.NotificationType == NotificationType.QuestionAnswered
                                && x.Answer.Question.QuestionId == id
                            )
                            ||
                            (
                                notificationLookupBy == NotificationLookupBy.ModerationId
                                && x.NotificationType == NotificationType.PostRejected
                                && x.Moderation.ModerationId == id
                            )
                        )
                        .Where(x => x.ActionedDateTime == null)
                        .Where(x => x.ForUser.UserId == (forUserId ?? x.ForUser.UserId))
                        .ToList();
        }
    }

    
}
