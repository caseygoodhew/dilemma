using System;
using System.Collections.Generic;

namespace Dilemma.Business.ViewModels
{
    public class CreateQuestionViewModel : QuestionViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
