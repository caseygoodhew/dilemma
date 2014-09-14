using System;

using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class UserService : IUserService
    {
        private static readonly Lazy<IUserRepository> UserRepository = new Lazy<IUserRepository>(Locator.Current.Instance<IUserRepository>);

        public int CreateAnonymousUser()
        {
            return UserRepository.Value.CreateAnonymousUser();
        }
    }
}
