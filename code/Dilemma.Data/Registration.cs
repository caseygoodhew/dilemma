using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Data
{
    public static class Registration
    {
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<IAdministrationRepository>(() => new AdministrationRepository());
            registrar.Register<IQuestionRepository>(() => new QuestionRepository());
            registrar.Register<ISiteRepository>(() => new SiteRepository());

            ConverterFactory.Register<Question>(registrar);
            ConverterFactory.Register<Answer>(registrar);
            ConverterFactory.Register<Category>(registrar);
            ConverterFactory.Register<SystemConfiguration>(registrar);

            EntityFramework.DilemmaContext.Startup();
        }
    }
}
