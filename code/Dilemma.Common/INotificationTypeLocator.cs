namespace Dilemma.Common
{
    /// <summary>
    /// Notification type locator interface.
    /// </summary>
    public interface INotificationTypeLocator
    {
        /// <summary>
        /// Gets the <see cref="NotificationRoute"/> for the given <see cref="NotificationType"/>.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType"/></param>
        /// <returns>The <see cref="NotificationRoute"/>.</returns>
        NotificationRoute GetRoute(NotificationType notificationType);
    }
}