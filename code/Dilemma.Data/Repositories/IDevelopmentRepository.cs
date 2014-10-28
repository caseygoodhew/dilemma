using System.Collections.Generic;

using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// The development repository provides access to internal development only resources.
    /// </summary>
    public interface IDevelopmentRepository
    {
        /// <summary>
        /// Sets the name of the development user.
        /// </summary>
        /// <param name="userId">The <see cref="User"/> id to set the name of.</param>
        /// <param name="name">The name to set.</param>
        void SetUserName(int userId, string name);

        /// <summary>
        /// Gets a list of <see cref="T"/> (<see cref="DevelopmentUser"/>) for the corresponding <see cref="userIds"/>.
        /// </summary>
        /// <typeparam name="T">The output type to convert to.</typeparam>
        /// <param name="userIds">The <see cref="User"/> ids to get.</param>
        /// <returns>The converted <see cref="DevelopmentUser"/>s.</returns>
        IEnumerable<T> GetList<T>(IEnumerable<int> userIds) where T : class;

        /// <summary>
        /// Gets a single user <see cref="T"/> (<see cref="DevelopmentUser"/>) for the corresponding <see cref="userId"/>.
        /// </summary>
        /// <typeparam name="T">The output type to convert to.</typeparam>
        /// <param name="userId">The <see cref="User"/> id to get.</param>
        /// <returns>The converted <see cref="DevelopmentUser"/>.</returns>
        T Get<T>(int userId) where T : class;

        /// <summary>
        /// Gets a flag indicating if the <see cref="userId"/> is that of a user that could be a <see cref="DevelopmentUser"/>.
        /// </summary>
        /// <param name="userId">The <see cref="User"/> id to check.</param>
        /// <returns>What it says on the box.</returns>
        bool CanLoginAs(int userId);
    }
}