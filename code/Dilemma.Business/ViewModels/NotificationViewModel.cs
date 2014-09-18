using System;
using System.Collections.Generic;

using Dilemma.Common;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.ViewModels
{
    public class NotificationViewModel
    {
        private static readonly Lazy<INotificationTypeLocator> NotificationTypeLocator =
            new Lazy<INotificationTypeLocator>(Locator.Current.Instance<INotificationTypeLocator>);

        
        private NotificationType notificationType;

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

        public NotificationRoute NotificationRoute { get; private set; }
        
        public int RouteDataValue { get; set; }
        
        public DateTime CreatedDateTime { get; set; }

        public DateTime? ActionedDateTime { get; set; }

        public string GetMessage()
        {
            switch (NotificationType)
            {
                case NotificationType.QuestionAnswered:
                    return "x answers have been added to your question";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IDictionary<string, object> GetRouteData()
        {
            var rd = new Dictionary<string, object> { { NotificationRoute.RouteDataKey, RouteDataValue } };
            return rd;
        }
    }
}
