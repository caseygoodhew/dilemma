using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The answer view model.
    /// </summary>
    public class AnswerViewModel
    {
        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        public int? AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the answer text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user added this answer.
        /// </summary>
        public bool IsMyAnswer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the answer has passed moderation.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the answer has been rejected.
        /// </summary>
        public bool IsRejected { get; set; }

        /// <summary>
        /// Gets or sets the vote count for the answer.
        /// </summary>
        public int VoteCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user has voted for this answer.
        /// </summary>
        public bool HasMyVote { get; set; }

        public int UserLevel { get; set; }

        public bool IHaveFlagged { get; set; }
        public bool IsStarVote { get; set; }
        public bool IsPopularVote { get; set; }
    }
}
