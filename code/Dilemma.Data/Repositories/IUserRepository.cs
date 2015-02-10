using System.Data.Entity;

using Dilemma.Data.Models;

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

        /// <summary>
        /// Gets the <see cref="User"/> in the specified type. There must be a converter registered between <see cref="User"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The user id to retrieve.</param>
        /// <returns>The <see cref="User"/> converted to type T.</returns>
        T GetUser<T>(int userId) where T : class;
    }
}
