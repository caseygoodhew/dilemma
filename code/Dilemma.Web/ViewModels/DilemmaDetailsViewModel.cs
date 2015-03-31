using Dilemma.Business.ViewModels;

namespace Dilemma.Web.ViewModels
{
    public class DilemmaDetailsViewModel
    {
        public QuestionDetailsViewModel QuestionDetails { get; set; }

        public AnswerViewModel MyAnswer { get; set; }

        public SidebarViewModel Sidebar { get; set; }
    }
}