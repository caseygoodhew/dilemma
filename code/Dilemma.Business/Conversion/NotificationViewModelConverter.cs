using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    public static class NotificationViewModelConverter
    {
        public static NotificationViewModel FromNotification(Notification model)
        {
            return new NotificationViewModel
                       {
                           NotificationType = model.NotificationType,
                           AnswerId = model.Answer == null ? null : model.Answer.AnswerId as int?,
                           QuestionId = model.Question == null ? null : model.Question.QuestionId as int?,
                           ModerationId = model.Moderation == null ? null : model.Moderation.ModerationId as int?,
                           CreatedDateTime = model.CreatedDateTime,
                           ActionedDateTime = model.ActionedDateTime
                       };
        }
    }
}
