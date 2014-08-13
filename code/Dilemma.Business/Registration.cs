using Dilemma.Business.Conversion;
using Dilemma.Business.Services;
using Dilemma.Business.Validators;
using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;

using FluentValidation;

namespace Dilemma.Business
{
    public static class Registration
    {
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<IAdministrationService>(() => new AdministrationService());
            registrar.Register<IQuestionService>(() => new QuestionService());
            
            registrar.Register<IValidator<QuestionViewModel>>(() => new QuestionViewModelValidator());
            ConverterFactory.Register<Question, QuestionViewModel>(registrar, QuestionViewModelConverter.FromQuestion);
            ConverterFactory.Register<QuestionViewModel, Question>(registrar, QuestionViewModelConverter.ToQuestion);
            
            registrar.Register<IValidator<SystemConfigurationViewModel>>(() => new SystemConfigurationViewModelValidator());
            ConverterFactory.Register<SystemConfiguration, SystemConfigurationViewModel>(registrar, SystemConfigurationViewModelConverter.FromSystemConfiguration);
            ConverterFactory.Register<SystemConfigurationViewModel, SystemConfiguration>(registrar, SystemConfigurationViewModelConverter.ToSystemConfiguration);
        }
    }
}
