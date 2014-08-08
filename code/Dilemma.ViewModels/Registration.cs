using Dilemma.ViewModels.Validation;

using Disposable.Common.ServiceLocator;

using FluentValidation;

namespace Dilemma.ViewModels
{
    public static class Registration
    {
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<IValidator<QuestionViewModel>>(() => new QuestionViewModelValidator());
        }
    }
}
