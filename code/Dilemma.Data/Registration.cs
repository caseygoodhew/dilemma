using System;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.MessagePipe;
using Disposable.MessagePipe.ServiceLocator;

namespace Dilemma.Data
{
    /// <summary>
    /// Registers data services with the <see cref="ILocator"/> <see cref="IRegistrar"/>.
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Registers data services with the <see cref="ILocator"/> <see cref="IRegistrar"/>.
        /// </summary>
        /// <param name="registrar">The <see cref="IRegistrar"/> to use.</param>
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<IAdministrationRepository>(() => new AdministrationRepository());
            registrar.Register<IQuestionRepository>(() => new QuestionRepository());
            registrar.Register<IInternalQuestionRepository>(() => new QuestionRepository());
            registrar.Register<ISiteRepository>(() => new SiteRepository());
            registrar.Register<IInternalUserRepository>(() => new UserRepository());
            registrar.Register<IUserRepository>(() => new UserRepository());
            registrar.Register<IDevelopmentRepository>(() => new DevelopmentRepository());
            registrar.Register<INotificationRepository>(() => new NotificationRepository());
            registrar.Register<IInternalNotificationRepository>(() => new NotificationRepository());
            registrar.Register<IInternalNotificationDistributor>(() => new NotificationDistributor());
            registrar.Register<IManualModerationRepository>(() => new ManualModerationRepository());
            registrar.Register<IInternalManualModerationRepository>(() => new ManualModerationRepository());
            registrar.Register<IInternalPointsRepository>(() => new PointsRepository());
            registrar.Register<IInternalPointDistributor>(() => new PointDistributor());

            registrar.CreatePipe<QuestionDataAction>(MessengerType.Stepping, InitiateMessagePipe);
            registrar.CreatePipe<AnswerDataAction>(MessengerType.Stepping, InitiateMessagePipe);
            registrar.CreatePipe<ModerationState>(MessengerType.Stepping, InitiateMessagePipe);
            
            ConverterFactory.Register<Question>(registrar);
            ConverterFactory.Register<Answer>(registrar);
            ConverterFactory.Register<Category>(registrar);
            ConverterFactory.Register<SystemConfiguration>(registrar);
            ConverterFactory.Register<ServerConfiguration>(registrar);
            ConverterFactory.Register<Notification>(registrar);
            ConverterFactory.Register<PointConfiguration>(registrar);

            DilemmaContext.Startup();
        }

        private static void InitiateMessagePipe(IMessagePipe<QuestionDataAction> messagePipe)
        {
            messagePipe.Locator<QuestionDataAction, IInternalManualModerationRepository>(QuestionDataAction.Created, x => x.OnQuestionCreated);
        }

        private static void InitiateMessagePipe(IMessagePipe<AnswerDataAction> messagePipe)
        {
            messagePipe.Locator<AnswerDataAction, IInternalManualModerationRepository>(AnswerDataAction.Created, x => x.OnAnswerCreated);
            messagePipe.Locator<AnswerDataAction, IInternalNotificationDistributor>(AnswerDataAction.StateChanged, x => x.OnAnswerStateChange);
        }

        private static void InitiateMessagePipe(IMessagePipe<ModerationState> messagePipe)
        {
            messagePipe.Locator<ModerationState, IInternalQuestionRepository>(ModerationState.Approved, x => x.OnModerationStateUpdated);
            messagePipe.Locator<ModerationState, IInternalPointDistributor>(ModerationState.Approved, x => x.OnModerationApproved);

            messagePipe.Locator<ModerationState, IInternalQuestionRepository>(ModerationState.Rejected, x => x.OnModerationStateUpdated);
            messagePipe.Locator<ModerationState, IInternalNotificationDistributor>(ModerationState.Rejected, x => x.OnModerationRejected);
        }
    }
}
