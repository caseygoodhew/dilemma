using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The question model.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the question created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the question closing date time.
        /// </summary>
        public DateTime ClosesDateTime { get; set; }

        /// <summary>
        /// Gets or sets the question <see cref="Category"/>.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of answers for the question.
        /// </summary>
        public int MaxAnswers { get; set; }

        /// <summary>
        /// Gets or sets the total number of answers against the question.
        /// </summary>
        public int TotalAnswers { get; set; }

        /// <summary>
        /// Gets or sets the list of completed answers against the question.
        /// </summary>
        public virtual List<Answer> Answers { get; set; }
    }
}
