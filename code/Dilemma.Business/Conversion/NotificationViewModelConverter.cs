using System.Security.Policy;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    public static class NotificationViewModelConverter
    {
        public static NEWNotificationViewModel FromSystemConfiguration(Notification model)
        {
            return new NEWNotificationViewModel
                       {
                           NotificationType = model.NotificationType,
                           AnswerId = model.Answer == null ? null : model.Answer.AnswerId as int?,
                           QuestionId = model.Answer == null ? null : model.Answer.Question.QuestionId as int?,
                           CreatedDateTime = model.CreatedDateTime,
                           ActionedDateTime = model.ActionedDateTime
                       };
        }
    }
}
