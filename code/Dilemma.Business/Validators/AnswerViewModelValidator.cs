using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates the <see cref="AnswerViewModel"/>.
    /// </summary>
    public class AnswerViewModelValidator : AbstractValidator<AnswerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerViewModelValidator"/> class.
        /// </summary>
        public AnswerViewModelValidator()
        {
            RuleFor(x => x.Text).NotNull().TidiedTrimmedLength(20, 500).WebPurify();
        }
    }
}
