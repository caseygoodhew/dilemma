using System;
using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The moderation model.
    /// </summary>
    public class Moderation
    {
        /// <summary>
        /// Gets or sets the moderation id.
        /// </summary>
        public int ModerationId { get; set; }

        /// <summary>
        /// Gets or sets the corresponding <see cref="User"/> that is being moderated.
        /// </summary>
        public User ForUser { get; set; }
        
        /// <summary>
        /// Gets or sets the moderation target (Question, Answer, etc.)
        /// </summary>
        public ModerationFor ModerationFor { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="Question"/> being moderated.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Answer"/> being moderated.
        /// </summary>
        public Answer Answer { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Followup"/> being moderated.
        /// </summary>
        public Followup Followup { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ModerationEntry"/>s that are attached to this moderation.
        /// </summary>
        public virtual List<ModerationEntry> ModerationEntries { get; set; }
    }
}
