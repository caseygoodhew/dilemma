using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Web.ViewModels
{
    public class AskViewModel
    {
        public QuestionViewModel Question { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public SidebarViewModel Sidebar { get; set; }
    }
}