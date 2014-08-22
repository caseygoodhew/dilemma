using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    public class AnswerViewModelValidator : AbstractValidator<AnswerViewModel>
    {
        public AnswerViewModelValidator()
        {
            RuleFor(x => x.Text).NotNull().TrimmedLength(20, 500);
        }
    }
}
