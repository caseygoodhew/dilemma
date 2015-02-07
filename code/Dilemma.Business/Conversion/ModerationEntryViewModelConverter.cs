using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="ModerationEntryViewModel"/>.
    /// </summary>
    public static class ModerationEntryViewModelConverter
    {
        /// <summary>
        /// Converts an <see cref="ModerationEntry"/> to an <see cref="ModerationEntryViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="ModerationEntry"/> to convert.</param>
        /// <returns>The resultant <see cref="ModerationEntryViewModel"/>.</returns>
        public static ModerationEntryViewModel FromModerationEntry(ModerationEntry model)
        {
            return new ModerationEntryViewModel
                       {
                           CreatedDateTime = model.CreatedDateTime,
                           State = model.State,
                           Message = model.Message
                       };
        }
    }

    /// <summary>
    /// Converts to and from the <see cref="UserViewModel"/>.
    /// </summary>
    public static class UserViewModelConverter
    {
        /// <summary>
        /// Converts an <see cref="User"/> to an <see cref="UserViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="User"/> to convert.</param>
        /// <returns>The resultant <see cref="UserViewModel"/>.</returns>
        public static UserViewModel FromUser(User model)
        {
            return new UserViewModel
            {
                UserId = model.UserId,
                Points = model.TotalPoints
            };
        }
    }
}