using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(x => x.Text).TrimmedLength(50, 500);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
