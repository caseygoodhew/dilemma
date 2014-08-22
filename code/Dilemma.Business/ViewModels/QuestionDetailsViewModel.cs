using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Business.ViewModels
{
    public class QuestionDetailsViewModel
    {
        public QuestionViewModel QuestionViewModel { get; set; }

        public bool CanAnswer { get; set; }

        public AnswerViewModel Answer { get; set; }
    }
}
