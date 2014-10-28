using System.Collections.Generic;
using System.Linq;

namespace Dilemma.Common
{
       /// <summary>
    /// Notification type locator class.
    /// </summary>
    public class NotificationTypeLocator : INotificationTypeLocator
    {
        private readonly IDictionary<NotificationType, NotificationRoute> routeLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeLocator"/> class.
        /// </summary>
        /// <param name="routes">All <see cref="NotificationRoute"/>s that will be registered.</param>
        public NotificationTypeLocator(params NotificationRoute[] routes)
        {
            routeLookup = routes.ToDictionary(x => x.NotificationType);
        }

        /// <summary>
        /// Gets the <see cref="NotificationRoute"/> for the given <see cref="NotificationType"/>.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType"/></param>
        /// <returns>The <see cref="NotificationRoute"/>.</returns>
        public NotificationRoute GetRoute(NotificationType notificationType)
        {
            return routeLookup[notificationType];
        }
    }
}