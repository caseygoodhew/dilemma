using System.Data.Entity;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Public user repository methods.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates an anonymous user.
        /// </summary>
        /// <returns>The id of the new user.</returns>
        int CreateAnonymousUser();
    }
}
