using Dilemma.Business.ViewModels;

using Disposable.Common.Extensions;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates the <see cref="QuestionViewModel"/>.
    /// </summary>
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionViewModelValidator"/> class.
        /// </summary>
        public QuestionViewModelValidator()
        {////"You dilemma should be between 50 and 500 characters"
            RuleFor(x => x.Text).NotNull().WithMessage("You must enter your dilemma").TidiedTrimmedLength(50, 500).WithMessage("Your dilemma must be between 50 and 500 characters. You entered {0} characters.", x => x.Text.TidyWhiteSpace().Trim().Length);
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("You must select a category");
        }
    }
}
