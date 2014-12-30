using System;

using Dilemma.Business.Services;

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
    }
}
