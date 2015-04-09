using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Notification view model converter.
    /// </summary>
    internal static class NotificationViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="Notification"/> to a <see cref="InternalNotificationViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Notification"/> to convert.</param>
        /// <returns>The <see cref="InternalNotificationViewModel"/>.</returns>
        internal static InternalNotificationViewModel FromNotification(Notification model)
        {
            return new InternalNotificationViewModel
                       {
                           NotificationType = model.NotificationType,
                           NotificationTarget = model.NotificationTarget,
                           AnswerId = model.Answer == null ? null : model.Answer.AnswerId as int?,
                           QuestionId = model.Question == null ? null : model.Question.QuestionId as int?,
                           ModerationId = model.Moderation == null ? null : model.Moderation.ModerationId as int?,
                           CreatedDateTime = model.CreatedDateTime,
                           ActionedDateTime = model.ActionedDateTime
                       };
        }
    }
}
