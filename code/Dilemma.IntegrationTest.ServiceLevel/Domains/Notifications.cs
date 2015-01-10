using System;
using System.Collections.Generic;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    public static class Notifications
    {
        private static readonly Lazy<INotificationService> _notificationService = Locator.Lazy<INotificationService>();
        
        public static IEnumerable<NotificationListViewModel> GetAll()
        {
            return _notificationService.Value.GetAll();
        }

        public static void Mute(NotificationLookupBy notificationLookupBy, int id)
        {
            _notificationService.Value.Mute(notificationLookupBy, id);
        }
    }
}