using System.Collections.Generic;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// Question list view model.
    /// </summary>
    public class QuestionListViewModel
    {
        /// <summary>
        /// Gets or sets the list of questions.
        /// </summary>
        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}
