using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Notification service interface.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Gets a list of all notifications for a user.
        /// </summary>
        /// <returns>A list of <see cref="NotificationViewModel"/>s.</returns>
        IEnumerable<NotificationListViewModel> GetAll();

        /// <summary>
        /// Mutes a notification.
        /// </summary>
        /// <param name="notificationLookupBy">The notification lookup type for the corresponding <see cref="id"/>.</param>
        /// <param name="id">The id of the item to mute notifications for.</param>
        void Mute(NotificationLookupBy notificationLookupBy, int id);
    }
}
