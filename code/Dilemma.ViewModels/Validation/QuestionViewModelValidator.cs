using FluentValidation;

namespace Dilemma.ViewModels.Validation
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(x => x.Text).NotEmpty();
        }
    }
}
