using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Web.ViewModels
{
    public class SidebarViewModel
    {
        public UserStatisticsViewModel UserStatsViewModel { get; set; }

        public int NewNotificationsCount { get; set; }

        public IEnumerable<NotificationListViewModel> Notifications { get; set; }
    }
}