using System.Collections;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Web.ViewModels
{
    public class DilemmasViewModel
    {
        public IEnumerable<QuestionViewModel> DilemmasToAnswer;

        public IEnumerable<QuestionViewModel> DilemmasToVote;
    }
}