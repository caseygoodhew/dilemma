using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Notification service.
    /// </summary>
    internal class NotificationService : INotificationService
    {
        private static readonly Lazy<INotificationRepository> NotificationRepository = Locator.Lazy<INotificationRepository>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        /// <summary>
        /// Gets a list of all notifications for a user.
        /// </summary>
        /// <returns>A list of <see cref="NotificationViewModel"/>s.</returns>
        public IEnumerable<NotificationListViewModel> GetAll()
        {
            var all = NotificationRepository.Value.GetAll<NotificationViewModel>(SecurityManager.Value.GetUserId()).ToList();
            
            var byQuestions = all.Where(x => x.QuestionId != null).GetGroupedQuestions();

            var remaining = all.Where(x => x.ModerationId != null).Select(x => new NotificationListViewModel
                                                                                 {
                                                                                     CreatedDateTime = x.CreatedDateTime,
                                                                                     IsActioned = x.ActionedDateTime != null,
                                                                                     RouteDataValue = x.ModerationId.Value,
                                                                                     NotificationType = x.NotificationType,
                                                                                     Occurrences = 1
                                                                                 });

            return byQuestions.Concat(remaining).OrderByDescending(x => x.CreatedDateTime);
        }

        /// <summary>
        /// Mutes a notification.
        /// </summary>
        /// <param name="notificationLookupBy">The notification lookup type for the corresponding <see cref="id"/>.</param>
        /// <param name="id">The id of the item to mute notifications for.</param>
        public void Mute(NotificationLookupBy notificationLookupBy, int id)
        {
            NotificationRepository.Value.Mute(SecurityManager.Value.GetUserId(), notificationLookupBy, id);
        }
    }
}