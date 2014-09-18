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
    /// <summary>
    /// Registers business services with the <see cref="ILocator"/> <see cref="IRegistrar"/>.
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Registers business services with the <see cref="ILocator"/> <see cref="IRegistrar"/>.
        /// </summary>
        /// <param name="registrar">The <see cref="IRegistrar"/> to use.</param>
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<IAdministrationService>(() => new AdministrationService());
            registrar.Register<IQuestionService>(() => new QuestionService());
            registrar.Register<ISiteService>(() => new SiteService());
            registrar.Register<IUserService>(() => new UserService());
            registrar.Register<IDevelopmentService>(() => new DevelopmentService());
            registrar.Register<INotificationService>(() => new NotificationService());

            registrar.Register<IValidator<CreateQuestionViewModel>>(() => new CreateQuestionViewModelValidator());
            //// keep this here as it will be used when we're out of testing
            //// registrar.Register<IValidator<CreateQuestionViewModel>>(() => new DerivativeValidator<CreateQuestionViewModel, QuestionViewModel>());
            ConverterFactory.Register<CreateQuestionViewModel, Question>(registrar, QuestionViewModelConverter.ToQuestion);

            registrar.Register<IValidator<QuestionViewModel>>(() => new QuestionViewModelValidator());
            ConverterFactory.Register<QuestionViewModel, Question>(registrar, QuestionViewModelConverter.ToQuestion);
            ConverterFactory.Register<Question, QuestionViewModel>(registrar, QuestionViewModelConverter.FromQuestion);

            registrar.Register<IValidator<QuestionDetailsViewModel>>(() => new QuestionDetailsViewModelValidator());
            ConverterFactory.Register<QuestionDetailsViewModel, Question>(registrar, QuestionDetailsViewModelConverter.ToQuestion);
            ConverterFactory.Register<Question, QuestionDetailsViewModel>(registrar, QuestionDetailsViewModelConverter.FromQuestion);

            registrar.Register<IValidator<AnswerViewModel>>(() => new AnswerViewModelValidator());
            ConverterFactory.Register<AnswerViewModel, Answer>(registrar, AnswerViewModelConverter.ToAnswer);
            ConverterFactory.Register<Answer, AnswerViewModel>(registrar, AnswerViewModelConverter.FromAnswer);
            
            registrar.Register<IValidator<SystemConfigurationViewModel>>(() => new SystemConfigurationViewModelValidator());
            ConverterFactory.Register<SystemConfigurationViewModel, SystemConfiguration>(registrar, SystemConfigurationViewModelConverter.ToSystemConfiguration);
            ConverterFactory.Register<SystemConfiguration, SystemConfigurationViewModel>(registrar, SystemConfigurationViewModelConverter.FromSystemConfiguration);

            ConverterFactory.Register<CategoryViewModel, Category>(registrar, CategoryViewModelConverter.ToCategory);
            ConverterFactory.Register<Category, CategoryViewModel>(registrar, CategoryViewModelConverter.FromCategory);

            ConverterFactory.Register<DevelopmentUserViewModel, DevelopmentUser>(registrar, DevelopmentUserViewModelConverter.ToDevelopmentUser);
            ConverterFactory.Register<DevelopmentUser, DevelopmentUserViewModel>(registrar, DevelopmentUserViewModelConverter.FromDevelopmentUser);

            ConverterFactory.Register<Notification, NotificationViewModel>(registrar, NotificationViewModelConverter.FromSystemConfiguration);
        }
    }
}
