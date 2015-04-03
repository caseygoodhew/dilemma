using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Web.ViewModels
{
    public class MyProfileViewModel
    {
        public IEnumerable<QuestionViewModel> Dilemmas;

        public IEnumerable<QuestionViewModel> Answers;

        public IEnumerable<NotificationListViewModel> Notifications;

        public SidebarViewModel Sidebar;
    }
}