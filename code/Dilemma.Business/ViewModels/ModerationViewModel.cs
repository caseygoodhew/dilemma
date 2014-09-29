using System.Collections.Generic;

namespace Dilemma.Business.ViewModels
{
    public class ModerationViewModel
    {
        public int ModerationId { get; set; }

        public List<ModerationEntryViewModel> ModerationEntries { get; set; }
    }
}