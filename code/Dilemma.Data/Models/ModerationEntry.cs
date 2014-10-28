using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The moderation entry model.
    /// </summary>
    public class ModerationEntry
    {
        /// <summary>
        /// Gets or sets the moderation entry id.
        /// </summary>
        public int ModerationEntryId { get; set; }

        /// <summary>
        /// Gets or sets the corresponding <see cref="Moderation"/>.
        /// </summary>
        public Moderation Moderation { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ModerationState"/>.
        /// </summary>
        public ModerationState State { get; set; }

        /// <summary>
        /// Gets or sets the moderation entry message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="User"/> that triggered the moderation entry to be created.
        /// </summary>
        public User AddedByUser { get; set; }
    }
}