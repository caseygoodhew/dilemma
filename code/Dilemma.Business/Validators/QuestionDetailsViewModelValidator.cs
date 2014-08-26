using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates the <see cref="QuestionDetailsViewModel"/>.
    /// </summary>
    public class QuestionDetailsViewModelValidator : AbstractValidator<QuestionDetailsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionDetailsViewModelValidator"/> class.
        /// </summary>
        public QuestionDetailsViewModelValidator()
        {
            RuleFor(x => x.Answer).SetValidator(new AnswerViewModelValidator());
        }
    }
}
