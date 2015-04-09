using System.Collections.Generic;

namespace Dilemma.Business.ViewModels
{
    public class QuestionModerationHistoryViewModel
    {
        public QuestionViewModel Question { get; set; }

        public IList<ModerationEntryViewModel> ModerationEntries { get; set; }
    }
}