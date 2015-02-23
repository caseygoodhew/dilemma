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
        /// Gets the system configuration as a <see cref="SystemServerConfigurationViewModel"/>.
        /// </summary>
        /// <returns>The <see cref="SystemServerConfigurationViewModel"/>.</returns>
        SystemServerConfigurationViewModel GetSystemServerConfiguration();

        /// <summary>
        /// Sets the system configuration as a <see cref="SystemServerConfigurationViewModel"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="SystemServerConfigurationViewModel"/> with the full set of changed and unchanged elements.</param>
        void SetSystemServerConfiguration(SystemServerConfigurationViewModel viewModel);

        TestingConfiguration GetTestingConfiguration();

        void SetTestingConfiguration(TestingConfiguration configuration);
        
        void SetPointConfiguration(PointConfigurationViewModel viewModel);

        PointConfigurationViewModel GetPointConfiguration(int id);
        
        PointConfigurationViewModel GetPointConfiguration(PointType pointType);

        IEnumerable<PointConfigurationViewModel> GetPointConfigurations();

        void ExpireAnswerSlots();
        
        void RetireOldQuestions();

        void CloseQuestions();

        LastRunLogViewModel GetLastRunLog();
    }
}
