using System;

using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// The user service interface.
    /// </summary>
    internal class UserService : IUserService
    {
        private static readonly Lazy<IUserRepository> UserRepository = Locator.Lazy<IUserRepository>();

        /// <summary>
        /// Creates a new anonymous user and returns the user id.
        /// </summary>
        /// <returns>The id of the new user.</returns>
        public int CreateAnonymousUser()
        {
            return UserRepository.Value.CreateAnonymousUser();
        }
    }
}
