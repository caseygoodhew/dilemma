using System;

using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// Moderation entry view model.
    /// </summary>
    public class ModerationEntryViewModel
    {
        /// <summary>
        /// Gets or sets the <see cref="ModerationState"/>.
        /// </summary>
        public ModerationState State { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the moderation entry was created.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the moderation entry message.
        /// </summary>
        public string Message { get; set; }
    }
}