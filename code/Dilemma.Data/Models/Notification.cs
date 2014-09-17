using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public User ForUser { get; set; }
        
        public NotificationType NotificationType { get; set; }

        public string NotificationTypeKey { get; set; }
        
        public string Controller { get; set; }

        public string Action { get; set; }

        public string RouteDataKey { get; set; }

        public string RouteDataValue { get; set; }
        
        public DateTime CreatedDateTime { get; set; }

        public DateTime? ActionedDateTime { get; set; }
    }
}
