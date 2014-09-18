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
        private static readonly Lazy<ITimeSource> TimeSource =
            new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);

        public IEnumerable<T> GetAll<T>(int forUserId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var notifications =
                    context.Notifications
                        .Include(x => x.Question)
                        .Where(x => x.ForUser.UserId == forUserId)
                        .Select(x => new
                                         {
                                             x.ActionedDateTime,
                                             x.CreatedDateTime,
                                             x.NotificationType,
                                             x.Question.QuestionId
                                         })
                        .OrderByDescending(x => x.CreatedDateTime)
                        .ToList()
                        .Select(x => new Notification
                                         {
                                             ActionedDateTime = x.ActionedDateTime,
                                             CreatedDateTime = x.CreatedDateTime,
                                             NotificationType = x.NotificationType,
                                             Question = new Question { QuestionId = x.QuestionId }
                                         })
                        .ToList();

                return ConverterFactory.ConvertMany<Notification, T>(notifications);
            }
        }

        public void Raise(int forUserId, NotificationType notificationType, int id)
        {
            using (var context = new DilemmaContext())
            {
                var notifications =
                    context.Notifications
                        .Where(notificationType, id)
                        .Where(x => x.ActionedDateTime == null)
                        .Where(x => x.ForUser.UserId == forUserId)
                        .ToList();

                if (notifications.Any())
                {
                    return;
                }

                var user = new User { UserId = forUserId };
                context.Users.Attach(user);

                var notification = new Notification
                        {
                            ForUser = user,
                            CreatedDateTime = TimeSource.Value.Now
                        };

                switch (notificationType)
                {
                    case NotificationType.QuestionAnswered:
                        notification.Question = new Question { QuestionId = id };
                        context.Questions.Attach(notification.Question);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("notificationType");
                }

                context.Notifications.Add(notification);
                    
                context.SaveChangesVerbose();
            }
        }

        public void Mute(int forUserId, NotificationType notificationType, int id)
        {
            using (var context = new DilemmaContext())
            {
                var notifications =
                    context.Notifications
                        .Where(notificationType, id)
                        .Where(x => x.ActionedDateTime == null)
                        .Where(x => x.ForUser.UserId == forUserId)
                        .ToList();

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

        public void Delete(int forUserId, NotificationType notificationType, int id)
        {
            using (var context = new DilemmaContext())
            {
                var notifications =
                    context.Notifications
                        .Where(notificationType, id)
                        .Where(x => x.ActionedDateTime == null)
                        .Where(x => x.ForUser.UserId == forUserId)
                        .ToList();

                notifications.ForEach(x => context.Entry(x).State = EntityState.Deleted);

                context.SaveChangesVerbose();
            }
        }
    }

    internal static class NotificationFilterExtensions
    {
        public static IQueryable<Notification> Where(this IQueryable<Notification> notification, NotificationType notificationType, int id)
        {           
            switch (notificationType)
            {
                case NotificationType.QuestionAnswered:
                    return
                        notification
                            .Where(x => x.NotificationType == notificationType)
                            .Where(x => x.Question.QuestionId == id);
                default:
                    throw new ArgumentOutOfRangeException("notificationType");
            }
        }   
    }
}
