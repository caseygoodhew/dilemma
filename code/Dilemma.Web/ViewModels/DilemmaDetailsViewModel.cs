using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Dilemma.Business.ViewModels;

namespace Dilemma.Web.ViewModels
{
    public class DilemmaDetailsViewModel
    {
        public QuestionDetailsViewModel QuestionDetails { get; set; }

        public SidebarViewModel Sidebar { get; set; }
    }
}