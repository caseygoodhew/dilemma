using Dilemma.Business.ViewModels;

using FluentValidation;
using FluentValidation.Results;

namespace Dilemma.Business.Validators
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            //RuleFor(x => x.Text).NotEmpty().Must(x => x.Length > 2).WithMessage("This is a test message for '{PropertyName}'.");
            RuleFor(x => x.Text).NotEmpty();
        }
    }
}
