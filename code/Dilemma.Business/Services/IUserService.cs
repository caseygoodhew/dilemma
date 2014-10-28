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
    }
}
