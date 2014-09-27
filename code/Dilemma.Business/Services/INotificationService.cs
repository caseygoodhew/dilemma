using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;

namespace Dilemma.Business.Services
{
    public interface INotificationService
    {
        IEnumerable<NotificationListViewModel> GetAll();

        void Mute(NotificationLookupBy notificationLookupBy, int id);
    }
}
