using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The question details view model.
    /// </summary>
    public class QuestionDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the question view model.
        /// </summary>
        public QuestionViewModel QuestionViewModel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  the user is allowed to answer the question.
        /// </summary>
        public bool CanAnswer { get; set; }

        /// <summary>
        /// Gets or sets the answer that the user is adding.
        /// </summary>
        public AnswerViewModel Answer { get; set; }
    }
}
