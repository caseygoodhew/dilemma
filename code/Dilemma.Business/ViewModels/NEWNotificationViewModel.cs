using System;

using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    public class NEWNotificationViewModel
    {
        public int NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public int? QuestionId { get; set; }

        public int? AnswerId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? ActionedDateTime { get; set; }
    }
}
