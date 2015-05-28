using System.Web.Configuration;
using Dilemma.Business.Conversion;
using Dilemma.Business.Services;
using Dilemma.Business.Utils;
using Dilemma.Business.Validators;
using Dilemma.Business.ViewModels;
using Dilemma.Business.WebPurify;
using Dilemma.Data.Models;
using Dilemma.Data.Models.Proxies;
using Dilemma.Data.Models.Virtual;

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
            registrar.Register<INotificationMessageFactory>(() => new NotificationMessageFactory());
            
            registrar.Register<IAdministrationService>(() => new AdministrationService());
            registrar.Register<IQuestionService>(() => new QuestionService());
            registrar.Register<ISiteService>(() => new SiteService());
            registrar.Register<IUserService>(() => new UserService());
            registrar.Register<IDevelopmentService>(() => new DevelopmentService());
            registrar.Register<INotificationService>(() => new NotificationService());
            registrar.Register<IManualModerationService>(() => new ManualModerationService());

            registrar.Register<IValidator<QuestionViewModel>>(() => new QuestionViewModelValidator());
            ConverterFactory.Register<QuestionViewModel, Question>(registrar, QuestionViewModelConverter.ToQuestion);
            ConverterFactory.Register<Question, QuestionViewModel>(registrar, QuestionViewModelConverter.FromQuestion);

            ConverterFactory.Register<QuestionDetailsViewModel, Question>(registrar, QuestionDetailsViewModelConverter.ToQuestion);
            ConverterFactory.Register<Question, QuestionDetailsViewModel>(registrar, QuestionDetailsViewModelConverter.FromQuestion);

            registrar.Register<IValidator<AnswerViewModel>>(() => new AnswerViewModelValidator());
            ConverterFactory.Register<AnswerViewModel, Answer>(registrar, AnswerViewModelConverter.ToAnswer);
            ConverterFactory.Register<Answer, AnswerViewModel>(registrar, AnswerViewModelConverter.FromAnswer);

            registrar.Register<IValidator<FollowupViewModel>>(() => new FollowupViewModelValidator());
            ConverterFactory.Register<FollowupViewModel, Followup>(registrar, FollowupViewModelConverter.ToFollowup);
            ConverterFactory.Register<Followup, FollowupViewModel>(registrar, FollowupViewModelConverter.FromFollowup);

            registrar.Register<IValidator<SystemConfigurationViewModel>>(() => new SystemConfigurationViewModelValidator());
            ConverterFactory.Register<SystemConfigurationViewModel, SystemConfiguration>(registrar, SystemConfigurationViewModelConverter.ToSystemConfiguration);
            ConverterFactory.Register<SystemConfiguration, SystemConfigurationViewModel>(registrar, SystemConfigurationViewModelConverter.FromSystemConfiguration);

            registrar.Register<IValidator<ServerConfigurationViewModel>>(() => new ServerConfigurationViewModelValidator());
            ConverterFactory.Register<ServerConfigurationViewModel, ServerConfiguration>(registrar, ServerConfigurationViewModelConverter.ToServerConfiguration);
            ConverterFactory.Register<ServerConfiguration, ServerConfigurationViewModel>(registrar, ServerConfigurationViewModelConverter.FromServerConfiguration);

            registrar.Register<IValidator<SystemServerConfigurationViewModel>>(() => new SystemServerConfigurationViewModelValidator());

            registrar.Register<IValidator<PointConfigurationViewModel>>(() => new PointConfigurationViewModelValidator());
            ConverterFactory.Register<PointConfigurationViewModel, PointConfiguration>(registrar, PointConfigurationViewModelConverter.ToPointConfiguration);
            ConverterFactory.Register<PointConfiguration, PointConfigurationViewModel>(registrar, PointConfigurationViewModelConverter.FromPointConfiguration);
            
            ConverterFactory.Register<CategoryViewModel, Category>(registrar, CategoryViewModelConverter.ToCategory);
            ConverterFactory.Register<Category, CategoryViewModel>(registrar, CategoryViewModelConverter.FromCategory);

            ConverterFactory.Register<DevelopmentUserViewModel, DevelopmentUser>(registrar, DevelopmentUserViewModelConverter.ToDevelopmentUser);
            ConverterFactory.Register<DevelopmentUser, DevelopmentUserViewModel>(registrar, DevelopmentUserViewModelConverter.FromDevelopmentUser);

            ConverterFactory.Register<Notification, InternalNotificationViewModel>(registrar, NotificationViewModelConverter.FromNotification);
            ConverterFactory.Register<Moderation, ModerationViewModel>(registrar, ModerationViewModelConverter.FromModeration);
            ConverterFactory.Register<ModerationEntry, ModerationEntryViewModel>(registrar, ModerationEntryViewModelConverter.FromModerationEntry);
            ConverterFactory.Register<User, UserViewModel>(registrar, UserViewModelConverter.FromUser);
            ConverterFactory.Register<LastRunLog, LastRunLogViewModel>(registrar, LastRunLogViewModelConverter.FromLastRunLog);
            ConverterFactory.Register<UserStatistics, UserStatisticsViewModel>(registrar, UserStatisticsViewModelConverter.FromUserStatistics);
            ConverterFactory.Register<Rank, RankViewModel>(registrar, RankViewModelConverter.FromRank);
            ConverterFactory.Register<QuestionHistoryProxy, QuestionModerationHistoryViewModel>(registrar, QuestionModerationHistoryViewModelConverter.FromQuestionHistoryProxy);
            ConverterFactory.Register<AnswerHistoryProxy, AnswerModerationHistoryViewModel>(registrar, AnswerModerationHistoryViewModelConverter.FromAnswerHistoryProxy);
            ConverterFactory.Register<FollowupHistoryProxy, FollowupModerationHistoryViewModel>(registrar, FollowupModerationHistoryViewModelConverter.FromFollowupHistoryProxy);

            registrar.Register<IWebPurifyResponder>(() => new WebPurifyResponder(WebConfigurationManager.AppSettings["WebPurifyApiKey"]));
        }
    }
}

