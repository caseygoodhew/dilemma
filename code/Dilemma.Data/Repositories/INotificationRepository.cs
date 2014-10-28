using System.Collections.Generic;

using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Publicly available notification repository methods.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Gets all notifications for a given user.
        /// </summary>
        /// <typeparam name="T">The type to convert the output <see cref="Notification"/>s to.</typeparam>
        /// <param name="forUserId">The userId to get the notifications for.</param>
        /// <returns>The converted <see cref="Notification"/>s.</returns>
        IEnumerable<T> GetAll<T>(int forUserId) where T : class;

        /// <summary>
        /// Mutes a notification for a given user.
        /// </summary>
        /// <param name="forUserId">The <see cref="User"/> id that the <see cref="Notification"/> belongs to.</param>
        /// <param name="notificationLookupBy">The <see cref="NotificationLookupBy"/>.</param>
        /// <param name="id">The id to use to lookup the notification.</param>
        void Mute(int forUserId, NotificationLookupBy notificationLookupBy, int id);
    }
}