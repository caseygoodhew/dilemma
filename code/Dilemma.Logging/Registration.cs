using System;
using System.Linq;
using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;
using NLog;

namespace Dilemma.Logging
{
    public static class Registration
    {
        /// <summary>
        /// Registers business services with the <see cref="ILocator"/> <see cref="IRegistrar"/>.
        /// </summary>
        /// <param name="registrar">The <see cref="IRegistrar"/> to use.</param>
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<ILogger>(() => new LoggerAdapter());

            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("substring", typeof (SubStringRendererWrapper));
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("emailsubject", typeof(EmailSubjectRendererWrapper));
        }
    }

    public static class Configuration
    {
        public static readonly string ServerGuid = Guid.NewGuid().ToString();
    }
}
