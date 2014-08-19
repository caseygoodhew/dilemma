using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.ServiceLocator;

using FluentValidation;
using FluentValidation.Internal;

namespace Dilemma.Business.Validators
{
    public class CreateQuestionViewModelValidator : AbstractValidator<CreateQuestionViewModel>
    {
        public CreateQuestionViewModelValidator()
        {
            RuleFor(x => x.MaxAnswers).InclusiveBetween(1, 10);
            RuleFor(x => x.QuestionLifetime).NotNull();

            AddRule(new DelegateValidator<QuestionViewModel>((item, context) =>
            {
                var result = Locator.Current.Instance<IValidator<QuestionViewModel>>().Validate(item);
                return result.Errors;
            }));
        }
    }
}
