using System;
using System.Linq;

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
        private static readonly Lazy<IProviderCache> Cache = new Lazy<IProviderCache>(Locator.Current.Instance<IProviderCache>);

        /// <summary>
        /// Sets the <see cref="SystemConfiguration"/> from the specified type. There must be a converter registered between <see cref="T"/> and <see cref="SystemConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="systemConfigurationType">The convertable instance.</param>
        public void SetSystemConfiguration<T>(T systemConfigurationType) where T : class
        {
            var systemConfiguration = ConverterFactory.ConvertOne<T, SystemConfiguration>(systemConfigurationType);

            var context = new DilemmaContext();
            context.SystemConfiguration.Update(context, systemConfiguration);
            context.SaveChangesVerbose();

            Cache.Value.Expire<SystemConfiguration>();
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
                        var context = new DilemmaContext();
                        return context.SystemConfiguration.Single();
                    });

            return ConverterFactory.ConvertOne<SystemConfiguration, T>(systemConfiguration);
        }
    }
}