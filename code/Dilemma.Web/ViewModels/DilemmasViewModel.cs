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
}