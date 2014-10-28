using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The notification model.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the notification id.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="User"/> that the notification is for.
        /// </summary>
        public User ForUser { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="NotificationType"/>.
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the corresponding <see cref="Moderation"/>.
        /// </summary>
        public Moderation Moderation { get; set; }
        
        /// <summary>
        /// Gets or sets the corresponding <see cref="Answer"/>.
        /// </summary>
        public Answer Answer { get; set; }

        /// <summary>
        /// Gets or sets the corresponding <see cref="Question"/>.
        /// </summary>
        public Question Question { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time that the notification was created.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the notification was actioned.
        /// </summary>
        public DateTime? ActionedDateTime { get; set; }
    }
}
