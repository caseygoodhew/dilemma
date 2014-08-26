using Dilemma.Business.ViewModels;

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
        {
            RuleFor(x => x.Text).NotNull().TidiedTrimmedLength(50, 500);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
