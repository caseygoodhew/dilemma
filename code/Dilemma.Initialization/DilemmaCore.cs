﻿using System;

using Disposable.Caching;
using Disposable.Common.ServiceLocator;

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

            locator.Initialize(
                Logging.Registration.Register,
                Business.Registration.Register,
                Data.Registration.Register,
                Security.Registration.Register,
                Disposable.Common.Registration.Register,
                RegisterShared);

            Data.Startup.Initialize();
        }

        /// <summary>
        /// Initializes the Core services.
        /// </summary>
        /// <returns>Flag indicating that the core started successfully.</returns>
        public static bool Initialize()
        {
            return DilemmaCoreInstance.Value != null;
        }

        private static void RegisterShared(IRegistrar registrar)
        {
            registrar.Register<IProviderCache>(() => new ProviderCache());
        }
    }
}
