using System;
using System.Collections.Generic;

using Dilemma.Business.Services;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel
{
    internal static class Users
    {
        private static readonly Lazy<IUserService> UserService = Locator.Lazy<IUserService>();

        private static IDictionary<string, int> userDictionary;

        public static void Reset()
        {
            userDictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        }

        public static int AnonymousUser(string reference)
        {
            int userId;

            if (!userDictionary.TryGetValue(reference, out userId))
            {
                userId = UserService.Value.CreateAnonymousUser();
                userDictionary.Add(reference, userId);
            }
            
            return userId;
        }
    }
}
