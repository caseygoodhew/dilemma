using System.Collections.Generic;
using System.Linq;

namespace Dilemma.Common
{
    public class NotificationTypeLocator : INotificationTypeLocator
    {
        private readonly IDictionary<NotificationType, NotificationRoute> routeLookup;

        public NotificationTypeLocator(params NotificationRoute[] routes)
        {
            routeLookup = routes.ToDictionary(x => x.NotificationType);
        }

        public NotificationRoute GetRoute(NotificationType notificationType)
        {
            return routeLookup[notificationType];
        }
    }
}
