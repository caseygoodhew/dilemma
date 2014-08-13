using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    public static class SystemConfigurationViewModelConverter
    {
        public static SystemConfiguration ToSystemConfiguration(SystemConfigurationViewModel viewModel)
        {
            return new SystemConfiguration
                       {
                           MaxAnswers = viewModel.MaxAnswers
                       };
        }

        public static SystemConfigurationViewModel FromSystemConfiguration(SystemConfiguration model)
        {
            return new SystemConfigurationViewModel
                       {
                           MaxAnswers = model.MaxAnswers
                       };
        }
    }
}
