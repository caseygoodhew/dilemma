using System;
using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    public class CreateQuestionViewModel : QuestionViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public QuestionLifetime QuestionLifetime { get; set; }

        public bool ShowTestingOptions { get; set; }
    }
}
