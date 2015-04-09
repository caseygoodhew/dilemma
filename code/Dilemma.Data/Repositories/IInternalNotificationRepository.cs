using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Notification repository methods that should not be externally available.
    /// </summary>
    internal interface IInternalNotificationRepository : INotificationRepository
    {
        /// <summary>
        /// Raises a new notification for a user.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="forUserId">The <see cref="User"/> that the notification should be created for.</param>
        /// <param name="notificationType">The <see cref="NotificationType"/>.</param>
        /// <param name="notificationTarget">The <see cref="NotificationTarget"/>.</param>
        /// <param name="lookupBy">The <see cref="NotificationLookupBy"/> for the <see cref="id"/>.</param>
        /// <param name="id">The id of the object that the notification refers to.</param>
        void Raise(DilemmaContext context, int forUserId, NotificationType notificationType, NotificationTarget notificationTarget, NotificationLookupBy lookupBy, int id);

        /// <summary>
        /// Deletes all existing notifications that refer to the provided <see cref="id"/>.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="notificationLookupBy">The <see cref="NotificationLookupBy"/>.</param>
        /// <param name="id">The id of the object that can be used to find the notification.</param>
        void Delete(DilemmaContext context, NotificationLookupBy notificationLookupBy, int id);
    }
}