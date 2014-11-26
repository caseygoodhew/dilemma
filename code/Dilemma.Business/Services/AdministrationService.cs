using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
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

        public void SetPointConfiguration(PointConfigurationViewModel viewModel)
        {
            AdministrationRepository.Value.SetPointConfiguration(viewModel);
        }

        public PointConfigurationViewModel GetPointConfiguration(int id)
        {
            return GetPointConfiguration((PointType)Enum.ToObject(typeof(PointType), id));
        }

        public PointConfigurationViewModel GetPointConfiguration(PointType pointType)
        {
            return AdministrationRepository.Value.GetPointConfiguration<PointConfigurationViewModel>(pointType);
        }

        public IEnumerable<PointConfigurationViewModel> GetPointConfigurations()
        {
            return AdministrationRepository.Value.GetPointConfigurations<PointConfigurationViewModel>();
        }
    }
}