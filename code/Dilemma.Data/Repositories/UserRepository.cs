using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// User repository implementation.
    /// </summary>
    internal class UserRepository : IInternalUserRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        /// <summary>
        /// Creates an anonymous user.
        /// </summary>
        /// <returns>The id of the new user.</returns>
        public int CreateAnonymousUser()
        {
            using (var context = new DilemmaContext())
            {
                var user = new User
                               {
                                   CreatedDateTime = TimeSource.Value.Now,
                                   UserType = UserType.Anonymous,
                                   HistoricPoints = 0,
                               };
                
                context.Users.Add(user);
                context.SaveChangesVerbose();
                
                return user.UserId;
            }
        }

        /// <summary>
        /// Gets the <see cref="User"/> in the specified type. There must be a converter registered between <see cref="User"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The user id to retrieve.</param>
        /// <returns>The <see cref="User"/> converted to type T.</returns>
        public T GetUser<T>(int userId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var user = context.Users.Single(x => x.UserId == userId);
                user.TotalPoints = GetTotalUserPoints(context, userId);
                return ConverterFactory.ConvertOne<User, T>(user);
            }
        }

        /// <summary>
        /// Gets the total number of points for a user.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/>.</param>
        /// <param name="userId">The user id to get the points for.</param>
        /// <returns>A dictionary of {UserId, TotalPoints}</returns>
        public int GetTotalUserPoints(DilemmaContext context, int userId)
        {
            return GetTotalUserPoints(context, new[] { userId })[userId];
        }

        /// <summary>
        /// Gets the total number of points against a set of user ids.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/>.</param>
        /// <param name="userIds">The user ids to get the points for.</param>
        /// <returns>A dictionary of {UserId, TotalPoints}</returns>
        /// <remarks>This dictionary result will always have all userIds as keys.</remarks>
        public IDictionary<int, int> GetTotalUserPoints(DilemmaContext context, IEnumerable<int> userIds)
        {
            var userIdList = userIds as IList<int> ?? userIds.ToList();

            if (!userIdList.Any())
            {
                return new Dictionary<int, int>();
            }

            var users = context.Users.Where(x => userIdList.Contains(x.UserId)).Select(
                x => new
                         {
                             x.UserId,
                             x.HistoricPoints
                         }).ToList();

            var points =
                context.UserPoints.Where(x => userIdList.Contains(x.ForUser.UserId))
                    .GroupBy(x => x.ForUser)
                    .ToDictionary(g => g.Key.UserId, g => g.Sum(x => x.PointsAwarded));

            return users.ToDictionary(
                x => x.UserId,
                x => x.HistoricPoints + (points.ContainsKey(x.UserId) ? points[x.UserId] : 0));
        }
    }
}
