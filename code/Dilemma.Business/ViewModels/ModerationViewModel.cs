using System.Collections.Generic;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// Moderation view model.
    /// </summary>
    public class ModerationViewModel
    {
        /// <summary>
        /// Gets or sets the moderation id.
        /// </summary>
        public int ModerationId { get; set; }

        /// <summary>
        /// Gets or sets the list of moderation entries related to this moderation.
        /// </summary>
        public List<ModerationEntryViewModel> ModerationEntries { get; set; }
    }
}