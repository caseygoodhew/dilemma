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
    internal class NotificationRepository : INotificationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        public IEnumerable<T> GetAll<T>(int forUserId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var notifications =
                    context.Notifications
                        .Include(x => x.Answer)
                        .Include(x => x.Answer.Question)
                        .Where(x => x.ForUser.UserId == forUserId)
                        .Select(x => new
                                         {
                                             x.ActionedDateTime,
                                             x.CreatedDateTime,
                                             x.NotificationType,
                                             x.Answer.AnswerId,
                                             x.Answer.Question.QuestionId
                                         })
                        .OrderByDescending(x => x.CreatedDateTime)
                        .ToList()
                        .Select(x => new Notification
                                         {
                                             ActionedDateTime = x.ActionedDateTime,
                                             CreatedDateTime = x.CreatedDateTime,
                                             NotificationType = x.NotificationType,
                                             Answer = new Answer { AnswerId = x.AnswerId, Question = new Question { QuestionId = x.QuestionId } }
                                         })
                        .ToList();

                return ConverterFactory.ConvertMany<Notification, T>(notifications);
            }
        }

        public void Raise(int forUserId, NotificationType notificationType, int id)
        {
            using (var context = new DilemmaContext())
            {
                var notification = new Notification
                        {
                            ForUser = new User { UserId = forUserId },
                            CreatedDateTime = TimeSource.Value.Now
                        };

                context.Users.Attach(notification.ForUser);

                switch (notificationType)
                {
                    case NotificationType.QuestionAnswered:
                        notification.Answer = new Answer { AnswerId = id };
                        context.Answers.Attach(notification.Answer);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("notificationType");
                }

                context.Notifications.Add(notification);
                
                context.SaveChangesVerbose();
            }
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

        public void Delete(int forUserId, NotificationLookupBy notificationLookupBy, int id)
        {
            using (var context = new DilemmaContext())
            {
                var notifications = FindNotifications(context, forUserId, notificationLookupBy, id);

                notifications.ForEach(x => context.Entry(x).State = EntityState.Deleted);

                context.SaveChangesVerbose();
            }
        }

        private List<Notification> FindNotifications(DilemmaContext context, int forUserId, NotificationLookupBy notificationLookupBy, int id)
        {
            return context.Notifications
                        .Where(
                            x =>
                            notificationLookupBy == NotificationLookupBy.QuestionId
                            && x.NotificationType == NotificationType.QuestionAnswered
                            && x.Answer.Question.QuestionId == id)
                        .Where(x => x.ActionedDateTime == null)
                        .Where(x => x.ForUser.UserId == forUserId)
                        .ToList();
        }
    }

    
}
