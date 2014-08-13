using System;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    class AdministrationService : IAdministrationService
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            new Lazy<IAdministrationRepository>(Locator.Current.Instance<IAdministrationRepository>);
        
        public SystemConfigurationViewModel GetSystemConfiguration()
        {
            return AdministrationRepository.Value.GetSystemConfiguration<SystemConfigurationViewModel>();
        }

        public void SetSystemConfiguration(SystemConfigurationViewModel viewModel)
        {
            AdministrationRepository.Value.SetSystemConfiguration(viewModel);
        }
    }
}