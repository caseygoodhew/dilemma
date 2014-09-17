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
    internal class NotificationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);

        private static readonly Lazy<INotificationTypeLocator> NotificationTypeLocator = new Lazy<INotificationTypeLocator>(Locator.Current.Instance<INotificationTypeLocator>);

        public IEnumerable<T> GetAll<T>(int forUserId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var notifications =
                    context.Notifications
                        .Where(x => x.ForUser.UserId == forUserId)
                        .OrderByDescending(x => x.CreatedDateTime)
                        .ToList();

                return ConverterFactory.ConvertMany<Notification, T>(notifications);
            }
        }
        
        public void Raise(int forUserId, NotificationType notificationType, string notificationTypeKey)
        {
            using (var context = new DilemmaContext())
            {
                var notifications = context.Notifications
                    .Where(x => x.NotificationType == notificationType)
                    .Where(x => x.NotificationTypeKey == notificationTypeKey)
                    .Where(x => x.ActionedDateTime == null)
                    .Where(x => x.ForUser.UserId == forUserId)
                    .ToList();

                if (notifications.Any())
                {
                    return;
                }

                var user = new User { UserId = forUserId };
                context.Users.Attach(user);

                var notificationRoute = NotificationTypeLocator.Value.GetRoute(notificationType);

                context.Notifications.Add(new Notification
                                              {
                                                  ForUser = user,
                                                  NotificationType = notificationType,
                                                  NotificationTypeKey = notificationTypeKey,
                                                  Controller = notificationRoute.Controller,
                                                  Action = notificationRoute.Action,
                                                  RouteDataKey = notificationRoute.RouteDataKey,
                                                  RouteDataValue = notificationTypeKey,
                                                  CreatedDateTime = TimeSource.Value.Now
                                              });

                context.SaveChangesVerbose();
            }
        }

        public void Mute(int forUserId, NotificationType notificationType, string notificationTypeKey)
        {
            using (var context = new DilemmaContext())
            {
                var notifications = context.Notifications
                    .Where(x => x.NotificationType == notificationType)
                    .Where(x => x.NotificationTypeKey == notificationTypeKey)
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

        public void Delete(int forUserId, NotificationType notificationType, string notificationTypeKey)
        {
            using (var context = new DilemmaContext())
            {
                var notifications = context.Notifications
                    .Where(x => x.NotificationType == notificationType)
                    .Where(x => x.NotificationTypeKey == notificationTypeKey)
                    .Where(x => x.ActionedDateTime == null)
                    .Where(x => x.ForUser.UserId == forUserId)
                    .ToList();

                notifications.ForEach(x => context.Entry(x).State = EntityState.Deleted);

                context.SaveChangesVerbose();
            }
        }
    }
}
