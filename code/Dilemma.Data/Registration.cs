using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;

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
            registrar.Register<IUserRepository>(() => new UserRepository());
            registrar.Register<IDevelopmentRepository>(() => new DevelopmentRepository());
            registrar.Register<INotificationRepository>(() => new NotificationRepository());
            registrar.Register<IInternalNotificationRepository>(() => new NotificationRepository());
            registrar.Register<IModerationRepository>(() => new ModerationRepository());
            registrar.Register<IInternalModerationRepository>(() => new ModerationRepository());
            
            ConverterFactory.Register<Question>(registrar);
            ConverterFactory.Register<Answer>(registrar);
            ConverterFactory.Register<Category>(registrar);
            ConverterFactory.Register<SystemConfiguration>(registrar);
            ConverterFactory.Register<Notification>(registrar);

            EntityFramework.DilemmaContext.Startup();
        }
    }
}
