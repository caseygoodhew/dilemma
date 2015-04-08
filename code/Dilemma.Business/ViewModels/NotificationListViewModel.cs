using System;
using System.Collections.Generic;

using Dilemma.Business.Utils;
using Dilemma.Common;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// Notification list view model.
    /// </summary>
    public class NotificationListViewModel
    {
        private static readonly Lazy<INotificationTypeLocator> NotificationTypeLocator = Locator.Lazy<INotificationTypeLocator>();

        private static readonly Lazy<INotificationMessageFactory> NotificationMessageFactory = Locator.Lazy<INotificationMessageFactory>();

        private NotificationType notificationType;

        /// <summary>
        /// Gets or sets the notification type.
        /// </summary>
        public NotificationType NotificationType
        {
            get
            {
                return notificationType;
            }

            set
            {
                notificationType = value;
                NotificationRoute = NotificationTypeLocator.Value.GetRoute(value); 
            }
        }

        public NotificationTarget NotificationTarget { get; set; }

        /// <summary>
        /// Gets the <see cref="NotificationRoute"/>.
        /// </summary>
        public NotificationRoute NotificationRoute { get; private set; }
        
        /// <summary>
        /// Gets or sets the Notification Route Data Value (e.g. the id of the question that the notification references.)
        /// </summary>
        public int RouteDataValue { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time that the most recent notification entry was created.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the notification has been actioned.
        /// </summary>
        public bool IsActioned { get; set; }

        /// <summary>
        /// Gets or sets the number of occurrences that this notification represents.
        /// </summary>
        public int Occurrences { get; set; }

        public int TotalPointsAwarded { get; set; }

        /// <summary>
        /// Gets the message relating to this notification.
        /// </summary>
        /// <returns>The notification message.</returns>
        public string GetMessage()
        {
            return NotificationMessageFactory.Value.GetMessage(NotificationType, NotificationTarget, Occurrences);
        }

        /// <summary>
        /// Gets a dictionary that can be used by MVC to create routing data.
        /// </summary>
        /// <returns>A dictionary that can be used by MVC to create routing data.</returns>
        public IDictionary<string, object> GetRouteData()
        {
            return new Dictionary<string, object> { { NotificationRoute.RouteDataKey, RouteDataValue } };
        }
    }

    
}
