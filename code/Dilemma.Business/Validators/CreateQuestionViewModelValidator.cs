using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.ServiceLocator;

using FluentValidation;
using FluentValidation.Internal;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates the <see cref="CreateQuestionViewModel"/>.
    /// </summary>
    public class CreateQuestionViewModelValidator : AbstractValidator<CreateQuestionViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuestionViewModelValidator"/> class.
        /// </summary>
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
