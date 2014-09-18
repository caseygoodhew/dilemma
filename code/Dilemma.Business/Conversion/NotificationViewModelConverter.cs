using System;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    public static class NotificationViewModelConverter
    {
        public static NotificationViewModel FromSystemConfiguration(Notification model)
        {
            return new NotificationViewModel
                       {
                           NotificationType = model.NotificationType,
                           RouteDataValue = GetRouteDataValue(model),
                           CreatedDateTime = model.CreatedDateTime,
                           ActionedDateTime = model.ActionedDateTime
                       };
        }

        private static int GetRouteDataValue(Notification model)
        {
            switch (model.NotificationType)
            {
                case NotificationType.QuestionAnswered:
                    return model.Question.QuestionId;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
