namespace Dilemma.Common
{
    public interface INotificationTypeLocator
    {
        NotificationRoute GetRoute(NotificationType notificationType);
    }
}