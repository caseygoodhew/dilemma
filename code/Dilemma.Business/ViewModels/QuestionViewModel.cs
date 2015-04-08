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
        /// Gets or sets the date and time that the question closes.
        /// </summary>
        public DateTime? ClosedDateTime { get; set; }

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

        public FollowupViewModel Followup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user asked this question.
        /// </summary>
        public bool IsMyQuestion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the question is approved.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the question is rejected.
        /// </summary>
        public bool IsRejected { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the question is open.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the question is closed.
        /// </summary>
        public bool IsClosed { get; set; }

        public bool IsBookmarked { get; set; }

        public bool HasFollowup { get; set; }

        public bool IHaveAnswsered { get; set; }

        public bool IHaveVoted { get; set; }
    }
}
