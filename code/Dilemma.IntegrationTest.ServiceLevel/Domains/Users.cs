using System;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    public static class Users
    {
        private static readonly Lazy<IUserService> _userService = Locator.Lazy<IUserService>();

        public static int AnonymousUser(string reference)
        {
            return ObjectDictionary.Get(ObjectType.User, reference, _userService.Value.CreateAnonymousUser);
        }

        public static UserViewModel GetUser(int userId)
        {
            return _userService.Value.GetUser(userId);
        }

        public static UserViewModel GetUser(string reference)
        {
            return GetUser
                (ObjectDictionary.Get<int>(ObjectType.User, reference));
        }
    }
}
