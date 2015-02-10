using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// The user service interface.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new anonymous user and returns the user id.
        /// </summary>
        /// <returns>The id of the new user.</returns>
        int CreateAnonymousUser();

        /// <summary>
        /// Gets the current user.
        /// </summary>
        UserViewModel GetCurrentUser();
        
        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">The user id to get.</param>
        /// <returns>The user view model.</returns>
        UserViewModel GetUser(int userId);
    }
}
