using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    public class QuestionDetailsViewModelValidator : AbstractValidator<QuestionDetailsViewModel>
    {
        public QuestionDetailsViewModelValidator()
        {
            // do we need this?
        }
    }
}
