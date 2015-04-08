using Dilemma.Common;

namespace Dilemma.Business.Utils
{
    internal interface INotificationMessageFactory
    {
        string GetMessage(NotificationType notificationType, NotificationTarget notificationTarget, int occurrences);
    }
}