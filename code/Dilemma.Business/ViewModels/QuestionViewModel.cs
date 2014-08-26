using System;
using System.Collections;
using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The question view model.
    /// </summary>
    public class QuestionViewModel
    {
        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        public int? QuestionId { get; set; }
        
        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the question was created.
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the question closes.
        /// </summary>
        public DateTime? ClosesDateTime { get; set; }

        /// <summary>
        /// Gets or sets the total number of (non-discarded) answers against this question. This value may not match the number of items in the <see cref="Answers"/> list.
        /// </summary>
        public int TotalAnswers { get; set; }
        
        /// <summary>
        /// Gets or sets the maximum number of answers allowed for this question.
        /// </summary>
        public int? MaxAnswers { get; set; }

        /// <summary>
        /// Gets or sets the question category id.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the question category name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the list of completed and approved answers for this question.
        /// </summary>
        public IList<AnswerViewModel> Answers { get; set; }
    }
}
