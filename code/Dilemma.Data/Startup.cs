using System;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data
{
    public static class Startup
    {
        private static bool isInitialized;
        
        public static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException("Already initialized");
            }

            var serverName = ServerName.Get();

            if (String.IsNullOrEmpty(serverName))
            {
                InitializeServerName();
            }
            
            isInitialized = true;
        }

        private static void InitializeServerName()
        {
            using (var context = new DilemmaContext())
            {
                var serverName = ServerName.GetNext(context.ServerConfiguration.Select(x => x.Name));

                var serverConfiguration = new ServerConfiguration
                                              {
                                                  Name = serverName,
                                                  ServerRole = ServerRole.NonProduction
                                              };

                context.ServerConfiguration.Add(serverConfiguration);

                context.SaveChangesVerbose();

                ServerName.Set(serverName);
            }
        }
    }
}
