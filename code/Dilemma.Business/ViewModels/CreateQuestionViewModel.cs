using System;
using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The view model for creating questions.
    /// </summary>
    public class CreateQuestionViewModel : QuestionViewModel
    {
        /// <summary>
        /// Gets or sets the list of <see cref="CategoryViewModel"/>s that the question can be assigned to.
        /// </summary>
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        /// <summary>
        /// Gets or sets the question lifetime (this value is only used in Development or Testing environments).
        /// </summary>
        public QuestionLifetime QuestionLifetime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page should show testing and development configuration options.
        /// </summary>
        public bool ShowTestingOptions { get; set; }
    }
}
