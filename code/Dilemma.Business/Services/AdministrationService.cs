using System;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Administration service provider.
    /// </summary>
    internal class AdministrationService : IAdministrationService
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();
        
        /// <summary>
        /// Gets the system configuration as a <see cref="SystemConfigurationViewModel"/>.
        /// </summary>
        /// <returns>The <see cref="SystemConfigurationViewModel"/>.</returns>
        public SystemConfigurationViewModel GetSystemConfiguration()
        {
            return AdministrationRepository.Value.GetSystemConfiguration<SystemConfigurationViewModel>();
        }

        /// <summary>
        /// Sets the system configuration as a <see cref="SystemConfigurationViewModel"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="SystemConfigurationViewModel"/> with the full set of changed and unchanged elements.</param>
        public void SetSystemConfiguration(SystemConfigurationViewModel viewModel)
        {
            AdministrationRepository.Value.SetSystemConfiguration(viewModel);
        }
    }
}