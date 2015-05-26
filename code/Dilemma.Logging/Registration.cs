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
            registrar.Register<ILogger>(() => new LoggerAdapter(LogManager.GetCurrentClassLogger()));

            Locator.Current.Instance<ILogger>().Info("Logger starting");
        }
    }
}
