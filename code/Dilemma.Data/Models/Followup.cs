using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class Followup
    {
        public int FollowupId { get; set; }

        /// <summary>
        /// Gets or sets the followup text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the followup created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the question that the followup belongs to.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// Gets or sets the followup state.
        /// </summary>
        public FollowupState FollowupState { get; set; }

        /// <summary>
        /// Gets or sets the user who created the followup.
        /// </summary>
        public User User { get; set; }

        public bool WebPurifyAttempted { get; set; }
        
        public bool PassedWebPurify { get; set; }
    }
}