using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;

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

            ValidateServerName();
            InitializeData();

            isInitialized = true;
        }

        private static void InitializeData()
        {
            using (var context = new DilemmaContext())
            {
                DilemmaInitializer.Seeder(context);
                context.SaveChangesVerbose();
            }
        }

        internal static void ValidateServerName()
        {
            var registryName = ServerName.Get();

            using (var context = new DilemmaContext())
            {
                if (!string.IsNullOrEmpty(registryName))
                {
                    var dbName =
                        context.ServerConfiguration.Where(x => x.Name == registryName)
                            .Select(x => x.Name)
                            .SingleOrDefault();

                    // if we have a regName and a dbName, we're good
                    // if we have a regName and no dbName, it's a new db instance
                    if (string.IsNullOrEmpty(dbName))
                    {
                        RegisterSeverName(context, registryName, false);
                    }
                }
                    // if we don't have a regName, it's a new server
                else
                {
                    RegisterSeverName(
                        context,
                        ServerName.GetNext(context.ServerConfiguration.Select(x => x.Name)),
                        true);
                }
            }
        }

        private static void RegisterSeverName(DilemmaContext context, string serverName, bool updateRegistry)
        {
            var serverConfiguration = new ServerConfiguration
            {
                Name = serverName,
                ServerRole = ServerRole.Offline
            };

            context.ServerConfiguration.Add(serverConfiguration);

            if (updateRegistry)
            {
                ServerName.Set(serverName);
            }

            context.SaveChangesVerbose();
        }
    }
}
