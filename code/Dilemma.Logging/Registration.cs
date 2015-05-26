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
        }
    }

    public static class Configuration
    {
        private static readonly string ServerGuid = Guid.NewGuid().ToString();

        private static string _serverName = ServerGuid;
        
        public static void SetServerName(string name)
        {
            var message = ServerGuid == _serverName
                ? string.Format("Setting server name to {0}", name)
                : string.Format(@"Updating server name from ""{0}"" to ""{1}""", _serverName, name);
            
            Locator.Get<ILogger>().Info(message);

            _serverName = name;
        }

        public static string GetServerName()
        {
            return ServerGuid == _serverName ? ServerGuid : string.Format("{1}: {0}", _serverName, ServerGuid);
        }
    }
}
