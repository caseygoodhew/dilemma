using System;

using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Initialization
{
    /// <summary>
    /// Singleton to register all core services with the service locator.
    /// </summary>
    public class DilemmaCore
    {
        private static readonly Lazy<DilemmaCore> DilemmaCoreInstance = new Lazy<DilemmaCore>(() => new DilemmaCore());

        private DilemmaCore()
        {
            var locator = Locator.Current as Locator;

            if (locator == null)
            {
                throw new InvalidOperationException();
            }

            var registrar = locator.BaseRegistrar;

            ViewModels.Registration.Register(registrar);

            // misc
            registrar.Register<ITimeSource>(() => new LocalTimeSource());
        }

        /// <summary>
        /// Initializes the Core services.
        /// </summary>
        /// <returns>Flag indicating that the core started successfully.</returns>
        public static bool Initialize()
        {
            return DilemmaCoreInstance.Value != null;
        }
    }
}
