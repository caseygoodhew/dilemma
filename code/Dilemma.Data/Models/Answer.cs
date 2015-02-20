using System;
using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The answer model/
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        public int AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the answer text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the answer created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the answer last touched date time.
        /// </summary>
        public DateTime LastTouchedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the question that the answer belongs to.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// Gets or sets the answer state.
        /// </summary>
        public AnswerState AnswerState { get; set; }

        /// <summary>
        /// Gets or sets the user who created the answer.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a list of users who have voted for this answer.
        /// </summary>
        public IList<int> UserVotes { get; set; }
    }
}
