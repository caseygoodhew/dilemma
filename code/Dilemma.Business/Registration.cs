using Dilemma.Business.Validators;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

using FluentValidation;

namespace Dilemma.Business
{
    public static class Registration
    {
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<IValidator<QuestionViewModel>>(() => new QuestionViewModelValidator());
        }
    }
}
