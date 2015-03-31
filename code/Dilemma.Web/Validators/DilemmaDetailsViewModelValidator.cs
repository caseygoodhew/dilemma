using Dilemma.Business.Validators;
using Dilemma.Web.ViewModels;

using FluentValidation;

namespace Dilemma.Web.Validators
{
    /// <summary>
    /// Validates the <see cref="DilemmaDetailsViewModel"/>.
    /// </summary>
    public class DilemmaDetailsViewModelValidator : AbstractValidator<DilemmaDetailsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DilemmaDetailsViewModel"/> class.
        /// </summary>
        public DilemmaDetailsViewModelValidator()
        {
            RuleFor(x => x.MyAnswer).SetValidator(new AnswerViewModelValidator());
        }
    }
}