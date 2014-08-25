using System;
using System.Linq;

using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Caching;
using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Data.Repositories
{
    internal class AdministrationRepository : IAdministrationRepository
    {
        private static readonly Lazy<IProviderCache> Cache = new Lazy<IProviderCache>(Locator.Current.Instance<IProviderCache>);
        
        public void SetSystemConfiguration<T>(T systemConfigurationType) where T : class
        {
            var systemConfiguration = ConverterFactory.ConvertOne<T, SystemConfiguration>(systemConfigurationType);

            var context = new DilemmaContext();
            context.SystemConfiguration.Update(context, systemConfiguration);
            context.SaveChangesVerbose();

            Cache.Value.Expire<SystemConfiguration>();
        }

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