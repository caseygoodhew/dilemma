using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.Extensions;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="DevelopmentUserViewModel"/>.
    /// </summary>
    public static class DevelopmentUserViewModelConverter
    {
        /// <summary>
        /// Converts an <see cref="DevelopmentUserViewModel"/> to an <see cref="DevelopmentUser"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="DevelopmentUserViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="DevelopmentUser"/>.</returns>
        public static DevelopmentUser ToDevelopmentUser(DevelopmentUserViewModel viewModel)
        {
            return new DevelopmentUser
                       {
                           UserId = viewModel.UserId.GuardedValue("UserId"),
                           Name = viewModel.Name
                       };
        }

        /// <summary>
        /// Converts an <see cref="DevelopmentUser"/> to an <see cref="DevelopmentUserViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="DevelopmentUser"/> to convert.</param>
        /// <returns>The resultant <see cref="DevelopmentUserViewModel"/>.</returns>
        public static DevelopmentUserViewModel FromDevelopmentUser(DevelopmentUser model)
        {
            return new DevelopmentUserViewModel
                {
                    UserId = model.UserId,
                    Name = model.Name
                };
        }
    }
}
