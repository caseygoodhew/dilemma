using System.Collections.Generic;

using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Internal user repository methods.
    /// </summary>
    internal interface IInternalUserRepository : IUserRepository
    {
        /// <summary>
        /// Gets the total number of points for a user.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/>.</param>
        /// <param name="userId">The user id to get the points for.</param>
        /// <returns>A dictionary of {UserId, TotalPoints}</returns>
        int GetTotalUserPoints(DilemmaContext context, int userId);
        
        /// <summary>
        /// Gets the total number of points against a set of user ids.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/>.</param>
        /// <param name="userIds">The user ids to get the points for.</param>
        /// <returns>A dictionary of {UserId, TotalPoints}</returns>
        IDictionary<int, int> GetTotalUserPoints(DilemmaContext context, IEnumerable<int> userIds);
    }
}