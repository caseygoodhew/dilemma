using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="PointConfigurationViewModel"/>.
    /// </summary>
    public static class PointConfigurationViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="PointConfigurationViewModel"/> to a <see cref="PointConfiguration"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="PointConfigurationViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="PointConfiguration"/>.</returns>
        public static PointConfiguration ToPointConfiguration(PointConfigurationViewModel viewModel)
        {
            return new PointConfiguration
                       {
                           PointType = viewModel.PointType,
                           Name = viewModel.Name,
                           Description = viewModel.Description,
                           Points = viewModel.Points
                       };
        }

        /// <summary>
        /// Converts a <see cref="PointConfiguration"/> to a <see cref="PointConfigurationViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="PointConfiguration"/> to convert.</param>
        /// <returns>The resultant <see cref="PointConfigurationViewModel"/>.</returns>
        public static PointConfigurationViewModel FromPointConfiguration(PointConfiguration model)
        {
            return new PointConfigurationViewModel
                       {
                           PointType = model.PointType,
                           Name = model.Name,
                           Description = model.Description,
                           Points = model.Points
                       };
        }
    }
}
