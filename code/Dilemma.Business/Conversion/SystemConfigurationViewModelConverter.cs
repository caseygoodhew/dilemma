using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="SystemConfigurationViewModel"/>.
    /// </summary>
    public static class SystemConfigurationViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="SystemConfigurationViewModel"/> to a <see cref="SystemConfiguration"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="SystemConfigurationViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="SystemConfiguration"/>.</returns>
        public static SystemConfiguration ToSystemConfiguration(SystemConfigurationViewModel viewModel)
        {
            return new SystemConfiguration
                       {
                           MaxAnswers = viewModel.MaxAnswers,
                           QuestionLifetime = viewModel.QuestionLifetime,
                           SystemEnvironment = viewModel.SystemEnvironment
                       };
        }

        /// <summary>
        /// Converts a <see cref="SystemConfiguration"/> to a <see cref="SystemConfigurationViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="SystemConfiguration"/> to convert.</param>
        /// <returns>The resultant <see cref="SystemConfigurationViewModel"/>.</returns>
        public static SystemConfigurationViewModel FromSystemConfiguration(SystemConfiguration model)
        {
            return new SystemConfigurationViewModel
                       {
                           MaxAnswers = model.MaxAnswers,
                           QuestionLifetime = model.QuestionLifetime,
                           SystemEnvironment = model.SystemEnvironment
                       };
        }
    }
}
