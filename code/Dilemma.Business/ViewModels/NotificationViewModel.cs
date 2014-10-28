using System;

using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// Notification view model.
    /// </summary>
    public class NotificationViewModel
    {
        /// <summary>
        /// Gets or sets the notification id.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the notification type.
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the corresponding question id.
        /// </summary>
        public int? QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the corresponding answer id.
        /// </summary>
        public int? AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the corresponding moderation id.
        /// </summary>
        public int? ModerationId { get; set; }

        /// <summary>
        /// Gets or sets the date and time that this notification was created.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time that this notification was actioned.
        /// </summary>
        public DateTime? ActionedDateTime { get; set; }
    }
}
