using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;

namespace Dilemma.Web.ViewModels
{
    public class DilemmasViewModel
    {
        public IEnumerable<QuestionViewModel> DilemmasToAnswer;

        public IEnumerable<QuestionViewModel> DilemmasToVote;

        public SidebarViewModel Sidebar;
    }

    public class SidebarViewModel
    {
        public UserStatsViewModel UserStatsViewModel;

        public IEnumerable<NotificationViewModel> Notifications;
    }

    public class AskViewModel
    {
        public QuestionViewModel Question;

        public IEnumerable<CategoryViewModel> Categories;

        public SidebarViewModel Sidebar;
    }
}