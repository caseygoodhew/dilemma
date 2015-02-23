using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;
using Dilemma.Data.Models.Virtual;

using Disposable.Caching;
using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Administration repository services implementation.
    /// </summary>
    internal class AdministrationRepository : IAdministrationRepository
    {
        private static readonly Lazy<IProviderCache> Cache = Locator.Lazy<IProviderCache>();

        /// <summary>
        /// Sets the <see cref="SystemConfiguration"/> from the specified type. There must be a converter registered between <see cref="T"/> and <see cref="SystemConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="systemConfigurationType">The convertable instance.</param>
        public void SetSystemConfiguration<T>(T systemConfigurationType) where T : class
        {
            var systemConfiguration = ConverterFactory.ConvertOne<T, SystemConfiguration>(systemConfigurationType);

            using (var context = new DilemmaContext())
            {
                context.SystemConfiguration.Update(context, systemConfiguration);
                context.SaveChangesVerbose();

                ExpireCache();
            }
        }

        /// <summary>
        /// Gets the <see cref="SystemConfiguration"/> in the specified type. There must be a converter registered between <see cref="SystemConfiguration"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>The <see cref="SystemConfiguration"/> converted to type T.</returns>
        public T GetSystemConfiguration<T>() where T : class
        {
            var systemConfiguration = Cache.Value.Get(
                () =>
                    {
                        using (var context = new DilemmaContext())
                        {
                            return context.SystemConfiguration.Single();
                        }
                    });

            return ConverterFactory.ConvertOne<SystemConfiguration, T>(systemConfiguration);
        }

        public void SetServerConfiguration<T>(T serverConfigurationType) where T : class
        {
            var systemConfiguration = ConverterFactory.ConvertOne<T, ServerConfiguration>(serverConfigurationType);
            
            using (var context = new DilemmaContext())
            {
                systemConfiguration.Name = ServerName.Get();
                systemConfiguration.Id = GetServerConfigurationId(context, systemConfiguration.Name);
                context.ServerConfiguration.Update(context, systemConfiguration);
                context.SaveChangesVerbose();

                ExpireCache();
            }
        }

        public T GetServerConfiguration<T>() where T : class
        {
            var serverName = ServerName.Get();
            
            var serverConfiguration = Cache.Value.Get(
                () =>
                {
                    using (var context = new DilemmaContext())
                    {
                        return context.ServerConfiguration.Single(x => x.Name == serverName);
                    }
                });

            return ConverterFactory.ConvertOne<ServerConfiguration, T>(serverConfiguration);
        }

        public void SetTestingConfiguration(TestingConfiguration configuration)
        {
            Cache.Value.Set(configuration);
        }

        public TestingConfiguration GetTestingConfiguration()
        {
            return Cache.Value.Get(() => new TestingConfiguration());
        }

        public void SetPointConfiguration<T>(T pointConfiguration) where T : class
        {
            var point = ConverterFactory.ConvertOne<T, PointConfiguration>(pointConfiguration);
            
            var systemConfiguration = GetSystemConfiguration<SystemConfiguration>();
            var testingConfiguration = GetTestingConfiguration();

            if (
                TestingConfiguration.NaturalComparison(
                    new TestingConfigurationContext(systemConfiguration.SystemEnvironment, testingConfiguration),
                    x => x.UseTestingPoints).Is(ActiveState.Active))
            {
                testingConfiguration.PointDictionary[point.PointType] = point.Points;
            }
            else
            {
                using (var context = new DilemmaContext())
                {
                    context.PointConfigurations.Update(context, point);
                    context.SaveChangesVerbose();

                    Cache.Value.Expire<List<PointConfiguration>>();
                }    
            }
        }

        public T GetPointConfiguration<T>(PointType pointType) where T : class
        {
            return ConverterFactory.ConvertOne<PointConfiguration, T>(GetPointConfigurations().Single(x => x.PointType == pointType)); 
        }

        public IEnumerable<T> GetPointConfigurations<T>() where T : class
        {
            return ConverterFactory.ConvertMany<PointConfiguration, T>(GetPointConfigurations());
        }

        public void ExpireAnswerSlots()
        {
            using (var context = new DilemmaContext())
            {
                // we don't expect a result, but if we don't ToList then the query doesn't execute
                var result = context.Database.SqlQuery<ExpireAnswerSlots>("ExpireAnswerSlots").ToList();
            }
        }
        
        public void RetireOldQuestions()
        {
            using (var context = new DilemmaContext())
            {
                // we don't expect a result, but if we don't ToList then the query doesn't execute
                var result = context.Database.SqlQuery<RetireOldQuestions>("RetireOldQuestions").ToList();
            }
        }

        public void CloseQuestions()
        {
            using (var context = new DilemmaContext())
            {
                // we don't expect a result, but if we don't ToList then the query doesn't execute
                var result = context.Database.SqlQuery<CloseQuestions>("CloseQuestions").ToList();
            }
        }

        public T GetLastRunLog<T>() where T : class
        {
            using (var context = new DilemmaContext())
            {
                var result = context.LastRunLog.SingleOrDefault();
                return ConverterFactory.ConvertOne<LastRunLog, T>(result);  
            }
        }

        private IEnumerable<PointConfiguration> GetPointConfigurations() 
        {
            var pointConfiguration = Cache.Value.Get(
                () =>
                {
                    using (var context = new DilemmaContext())
                    {
                        return context.PointConfigurations.ToList();
                    }
                });

            var systemConfiguration = GetSystemConfiguration<SystemConfiguration>();
            var testingConfiguration = GetTestingConfiguration();
            
            if (
                TestingConfiguration.NaturalComparison(
                    new TestingConfigurationContext(systemConfiguration.SystemEnvironment, testingConfiguration),
                    x => x.UseTestingPoints).Is(ActiveState.Active))
            {
                pointConfiguration.ForEach(
                    x =>
                        {
                            x.Points = testingConfiguration.PointDictionary[x.PointType];
                        });
            }
            
            
            return pointConfiguration;
        }

        private static void ExpireCache()
        {
            Cache.Value.Expire<SystemConfiguration>();
            
            Cache.Value.Expire<TestingConfiguration>();

            Cache.Value.Expire<ServerConfiguration>();
        }

        private static int GetServerConfigurationId(DilemmaContext context, string serverName)
        {
            return context.ServerConfiguration.Where(x => x.Name == serverName).Select(x => x.Id).Single();
        }
    }
}