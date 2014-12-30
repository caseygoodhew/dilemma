using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Administration service provider interface.
    /// </summary>
    public interface IAdministrationService
    {
        /// <summary>
        /// Gets the system configuration as a <see cref="SystemConfigurationViewModel"/>.
        /// </summary>
        /// <returns>The <see cref="SystemConfigurationViewModel"/>.</returns>
        SystemConfigurationViewModel GetSystemConfiguration();

        /// <summary>
        /// Sets the system configuration as a <see cref="SystemConfigurationViewModel"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="SystemConfigurationViewModel"/> with the full set of changed and unchanged elements.</param>
        void SetSystemConfiguration(SystemConfigurationViewModel viewModel);

        TestingConfiguration GetTestingConfiguration();

        void SetTestingConfiguration(TestingConfiguration configuration);
        
        void SetPointConfiguration(PointConfigurationViewModel viewModel);

        PointConfigurationViewModel GetPointConfiguration(int id);
        
        PointConfigurationViewModel GetPointConfiguration(PointType pointType);

        IEnumerable<PointConfigurationViewModel> GetPointConfigurations();
    }
}
