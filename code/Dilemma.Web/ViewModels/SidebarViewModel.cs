using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Web.ViewModels
{
    public class SidebarViewModel
    {
        public UserStatsViewModel UserStatsViewModel;

        public IEnumerable<NotificationViewModel> Notifications;
    }
}