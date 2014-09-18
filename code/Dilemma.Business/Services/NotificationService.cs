using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class NotificationService : INotificationService
    {
        private static readonly Lazy<INotificationRepository> NotificationRepository = new Lazy<INotificationRepository>(Locator.Current.Instance<INotificationRepository>);

        private static readonly Lazy<ISecurityManager> SecurityManager = new Lazy<ISecurityManager>(Locator.Current.Instance<ISecurityManager>);

        public IEnumerable<NotificationViewModel> GetAll()
        {
            return NotificationRepository.Value.GetAll<NotificationViewModel>(SecurityManager.Value.GetUserId());
        }

        public void Mute(NotificationType notificationType, int id)
        {
            NotificationRepository.Value.Mute(SecurityManager.Value.GetUserId(), notificationType, id);
        }
    }
}