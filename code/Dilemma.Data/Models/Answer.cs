using System;

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
        /// Gets or sets the question that the answer belongs to.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// Gets or sets the answer type.
        /// </summary>
        public AnswerType AnswerType { get; set; }
    }
}
