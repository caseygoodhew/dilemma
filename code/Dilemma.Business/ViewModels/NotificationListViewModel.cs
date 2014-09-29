using System;
using System.Collections.Generic;

using Dilemma.Common;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.ViewModels
{
    public class NotificationListViewModel
    {
        private static readonly Lazy<INotificationTypeLocator> NotificationTypeLocator = Locator.Lazy<INotificationTypeLocator>();
        
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
        
        public DateTime DateTime { get; set; }

        public bool IsActioned { get; set; }

        public int Occurrences { get; set; }

        public string GetMessage()
        {
            switch (NotificationType)
            {
                case NotificationType.QuestionAnswered:
                    return Occurrences == 1
                               ? string.Format("1 answer has been added to your question.")
                               : string.Format("{0} answers have been added to your question.", Occurrences);
                case NotificationType.PostRejected:
                    return "Your post has been rejected.";

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
