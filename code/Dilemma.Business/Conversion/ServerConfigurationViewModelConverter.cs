using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="ServerConfigurationViewModel"/>.
    /// </summary>
    public static class ServerConfigurationViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="ServerConfigurationViewModel"/> to a <see cref="ServerConfiguration"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="ServerConfigurationViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="ServerConfiguration"/>.</returns>
        public static ServerConfiguration ToServerConfiguration(ServerConfigurationViewModel viewModel)
        {
            return new ServerConfiguration
                       {
                           ServerRole = viewModel.ServerRole
                       };
        }

        /// <summary>
        /// Converts a <see cref="ServerConfiguration"/> to a <see cref="ServerConfigurationViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="ServerConfiguration"/> to convert.</param>
        /// <returns>The resultant <see cref="ServerConfigurationViewModel"/>.</returns>
        public static ServerConfigurationViewModel FromServerConfiguration(ServerConfiguration model)
        {
            return new ServerConfigurationViewModel
                       {
                           ServerRole = model.ServerRole
                       };
        }
    }
}
