using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;

namespace Dilemma.Business.Services
{
    public interface INotificationService
    {
        IEnumerable<NotificationViewModel> GetAll();

        void Mute(NotificationType notificationType, int id);
    }
}
