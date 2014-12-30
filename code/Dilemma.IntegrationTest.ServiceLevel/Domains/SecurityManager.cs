using System;

using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    internal static class SecurityManager
    {
        private static readonly Lazy<ISecurityManager> _securityManager = Locator.Lazy<ISecurityManager>();

        public static void ValidateClaims()
        {
            _securityManager.Value.ValidateClaims();
        }
        
        public static int LoginNewAnonymous(string reference)
        {
            return ObjectDictionary.Get(ObjectType.User, reference, _securityManager.Value.LoginNewAnonymous);
        }

        public static int GetUserId()
        {
            return _securityManager.Value.GetUserId();
        }

        public static void SetUserId(int userId)
        {
            _securityManager.Value.SetUserId(userId);
        }

        public static void SetUserId(string reference)
        {
            SetUserId(ObjectDictionary.Get<int>(ObjectType.User, reference));
        }
    }
}