using System.Data.Entity;
using System.Linq;

using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;

namespace Dilemma.Data.Repositories
{
    internal class AdministrationRepository : IAdministrationRepository
    {
        public void SetSystemConfiguration<T>(T systemConfigurationType) where T : class
        {
            var systemConfiguration = ConverterFactory.ConvertOne<T, SystemConfiguration>(systemConfigurationType);

            var context = new DilemmaContext();
            context.SystemConfiguration.Update(context, systemConfiguration);
            context.SaveChanges();
        }

        public T GetSystemConfiguration<T>() where T : class
        {
            var context = new DilemmaContext();
            return ConverterFactory.ConvertOne<SystemConfiguration, T>(context.SystemConfiguration.Single());
        }
    }
}