using Dilemma.Business.Validators;
using Dilemma.Web.ViewModels;

using FluentValidation;

namespace Dilemma.Web.Validators
{
    /// <summary>
    /// Validates the <see cref="AskViewModel"/>.
    /// </summary>
    public class AskViewModelValidator : AbstractValidator<AskViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AskViewModelValidator"/> class.
        /// </summary>
        public AskViewModelValidator()
        {
            RuleFor(x => x.Question).SetValidator(new QuestionViewModelValidator());
        }
    }
}
