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
        /// Gets the system configuration as a <see cref="SystemServerConfigurationViewModel"/>.
        /// </summary>
        /// <returns>The <see cref="SystemServerConfigurationViewModel"/>.</returns>
        public SystemServerConfigurationViewModel GetSystemServerConfiguration()
        {
            return new SystemServerConfigurationViewModel
                       {
                           ServerConfigurationViewModel = AdministrationRepository.Value.GetServerConfiguration<ServerConfigurationViewModel>(),
                           SystemConfigurationViewModel = AdministrationRepository.Value.GetSystemConfiguration<SystemConfigurationViewModel>()
                       };
        }

        /// <summary>
        /// Sets the system configuration as a <see cref="SystemServerConfigurationViewModel"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="SystemServerConfigurationViewModel"/> with the full set of changed and unchanged elements.</param>
        public void SetSystemServerConfiguration(SystemServerConfigurationViewModel viewModel)
        {
            AdministrationRepository.Value.SetSystemConfiguration(viewModel.SystemConfigurationViewModel);
            AdministrationRepository.Value.SetServerConfiguration(viewModel.ServerConfigurationViewModel);
        }

        public TestingConfiguration GetTestingConfiguration()
        {
            return AdministrationRepository.Value.GetTestingConfiguration();
        }

        public void SetTestingConfiguration(TestingConfiguration configuration)
        {
            AdministrationRepository.Value.SetTestingConfiguration(configuration);
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

        public void ExpireAnswerSlots()
        {
            AdministrationRepository.Value.ExpireAnswerSlots();
        }
        
        public void RetireOldQuestions()
        {
            AdministrationRepository.Value.RetireOldQuestions();
        }

        public LastRunLogViewModel GetLastRunLog()
        {
            return AdministrationRepository.Value.GetLastRunLog<LastRunLogViewModel>();
        }

        public bool IsOnline()
        {
            return AdministrationRepository.Value.GetServerConfiguration<ServerConfigurationViewModel>().ServerRole
                   != ServerRole.Offline;
        }

        public void ExpireCachedSystemServerConfiguration()
        {
            AdministrationRepository.Value.ExpireCachedSystemServerConfiguration();
        }
    }
}