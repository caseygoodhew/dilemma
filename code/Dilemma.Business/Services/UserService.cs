using System;

using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class UserService : IUserService
    {
        private static readonly Lazy<IUserRepository> UserRepository = Locator.Lazy<IUserRepository>();

        public int CreateAnonymousUser()
        {
            return UserRepository.Value.CreateAnonymousUser();
        }
    }
}
