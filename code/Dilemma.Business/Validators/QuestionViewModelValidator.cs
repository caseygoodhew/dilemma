using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(x => x.Text).NotNull().TidiedTrimmedLength(50, 500);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
