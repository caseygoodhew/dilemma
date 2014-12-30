using System;
using System.Linq;
using System.Security.Claims;

using Dilemma.Security;

using Owin;

namespace Dilemma.IntegrationTest.ServiceLevel.Support
{
    internal class TestAuthenticationManager : IAuthenticationManager
    {
        private UserClaims currentUserClaims;

        private int someNumber = new Random().Next(1000);
        
        public void Configure(IAppBuilder appBuilder)
        {
        }

        public void SignOut()
        {
            currentUserClaims = null;
        }

        public void SignIn(UserClaims claims)
        {
            currentUserClaims = claims;
        }

        public UserClaims GetUserClaims()
        {
            return currentUserClaims ?? (currentUserClaims = new UserClaims(Enumerable.Empty<Claim>()));
        }
    }
}