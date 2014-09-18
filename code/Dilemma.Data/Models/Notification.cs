using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public User ForUser { get; set; }
        
        public NotificationType NotificationType { get; set; }

        public Question Question { get; set; }
        
        public DateTime CreatedDateTime { get; set; }

        public DateTime? ActionedDateTime { get; set; }
    }
}
