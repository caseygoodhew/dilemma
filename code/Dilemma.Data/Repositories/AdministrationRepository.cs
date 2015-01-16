using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

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

                Cache.Value.Expire<SystemConfiguration>();
                // always reset the testing configuration as this should be used during integration tests anyway
                Cache.Value.Expire<TestingConfiguration>();
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

            using (var context = new DilemmaContext())
            {
                context.PointConfigurations.Update(context, point);
                context.SaveChangesVerbose();

                Cache.Value.Expire<List<PointConfiguration>>();
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

        private IEnumerable<PointConfiguration> GetPointConfigurations() 
        {
            return Cache.Value.Get(
                () =>
                {
                    using (var context = new DilemmaContext())
                    {
                        return context.PointConfigurations.ToList();
                    }
                });
        }
    }
}