using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// The development repository provides access to internal development only resources.
    /// </summary>
    internal class DevelopmentRepository : IDevelopmentRepository
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();

        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DevelopmentRepository"/> class.
        /// </summary>
        public DevelopmentRepository()
        {
            var systemConfiguration = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();
            
            if (!SystemEnvironmentValidation.IsInternalEnvironment(systemConfiguration.SystemEnvironment))
            {
                throw new UnauthorizedAccessException();
            }
        }

        /// <summary>
        /// Sets the name of the development user.
        /// </summary>
        /// <param name="userId">The <see cref="User"/> id to set the name of.</param>
        /// <param name="name">The name to set.</param>
        public void SetUserName(int userId, string name)
        {
            using (var context = new DilemmaContext())
            {
                var devUser = context.DevelopmentUsers.FirstOrDefault(x => x.UserId == userId);

                if (devUser == null || devUser.UserId != userId)
                {
                    devUser = new DevelopmentUser { UserId = userId, CreatedDateTime = TimeSource.Value.Now };

                    if (GetAnonymousUsers(context, new[] { devUser.UserId }).Any())
                    {
                        context.DevelopmentUsers.Add(devUser);
                    }
                }
                else
                {
                    context.Entry(devUser).State = EntityState.Modified;
                }

                devUser.Name = name;

                context.SaveChangesVerbose();
            }
        }

        /// <summary>
        /// Gets a list of <see cref="T"/> (<see cref="DevelopmentUser"/>) for the corresponding <see cref="userIds"/>.
        /// </summary>
        /// <typeparam name="T">The output type to convert to.</typeparam>
        /// <param name="userIds">The <see cref="User"/> ids to get.</param>
        /// <returns>The converted <see cref="DevelopmentUser"/>s.</returns>
        public IEnumerable<T> GetList<T>(IEnumerable<int> userIds) where T : class 
        {
            using (var context = new DilemmaContext())
            {
                var users = context.DevelopmentUsers.Where(x => userIds.Contains(x.UserId)).OrderByDescending(x => x.CreatedDateTime).ToList();

                var missingUsers = userIds.Where(x => !users.Select(u => u.UserId).Contains(x))
                                          .Select(x => new DevelopmentUser { UserId = x, CreatedDateTime = TimeSource.Value.Now })
                                          .ToList();

                if (missingUsers.Any())
                {
                    var validIds = GetAnonymousUsers(context, missingUsers.Select(x => x.UserId));
                    context.DevelopmentUsers.AddRange(missingUsers.Where(x => validIds.Contains(x.UserId)));
                    context.SaveChangesVerbose();
                }

                return ConverterFactory.ConvertMany<DevelopmentUser, T>(missingUsers.Concat(users));
            }
        }

        /// <summary>
        /// Gets a single user <see cref="T"/> (<see cref="DevelopmentUser"/>) for the corresponding <see cref="userId"/>.
        /// </summary>
        /// <typeparam name="T">The output type to convert to.</typeparam>
        /// <param name="userId">The <see cref="User"/> id to get.</param>
        /// <returns>The converted <see cref="DevelopmentUser"/>.</returns>
        public T Get<T>(int userId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return ConverterFactory.ConvertOne<DevelopmentUser, T>(context.DevelopmentUsers.FirstOrDefault(x => x.UserId == userId));
            }
        }

        /// <summary>
        /// Gets a flag indicating if the <see cref="userId"/> is that of a user that could be a <see cref="DevelopmentUser"/>.
        /// </summary>
        /// <param name="userId">The <see cref="User"/> id to check.</param>
        /// <returns>What it says on the box.</returns>
        public bool CanLoginAs(int userId)
        {
            using (var context = new DilemmaContext())
            {
                var user = context.Users.FirstOrDefault(x => x.UserId == userId);
                return user.UserType == UserType.Anonymous;
            }
        }

        private static IEnumerable<int> GetAnonymousUsers(DilemmaContext context, IEnumerable<int> userIds)
        {
            return
                context.Users.Where(x => userIds.Contains(x.UserId))
                    .Where(x => x.UserType == UserType.Anonymous)
                    .Select(x => x.UserId)
                    .ToList();
        }
    }
}
