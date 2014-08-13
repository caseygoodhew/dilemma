using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    public interface IAdministrationService
    {
        SystemConfigurationViewModel GetSystemConfiguration();

        void SetSystemConfiguration(SystemConfigurationViewModel viewModel);
    }
}
