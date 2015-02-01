using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web.Configuration;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;
using Disposable.Web.Caching;

using Owin;

namespace Dilemma.Security
{
    /// <summary>
    /// Security manager.
    /// </summary>
    internal class SecurityManager : ISecurityManager
    {
        private static readonly Lazy<IUserRepository> UserRepository = Locator.Lazy<IUserRepository>();

        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();
        
        private static readonly Lazy<IAuthenticationManager> AuthenticationManager = Locator.Lazy<IAuthenticationManager>();

        /// <summary>
        /// Configures authentication in the OWIN pipeline using the configured IAuthenticationManager.
        /// </summary>
        /// <param name="appBuilder">The OWIN app builder.</param>
        public void ConfigureAuthentication(IAppBuilder appBuilder)
        {
            AuthenticationManager.Value.Configure(appBuilder);
        }

        /// <summary>
        /// Validates the users security credentials.
        /// </summary>
        public void ValidateClaims()
        {
            var claims = ReadClaims().ToList();
            
            if (!HasClaims(claims))
            {
                var userId = CreateAnonymousUser();
                IssueAnonymousCookie(userId);
                return;
            }

            if (IsAnonymousCookie(claims))
            {
                return;
            }

            if (!IsSecureAction())
            {
                return;
            }

            if (IsSecurityStampValid(claims))
            {
                return;
            }

            RedirectToLogin();
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <returns>The current user id.</returns>
        public int GetUserId()
        {
            return int.Parse(ReadClaims().Single(x => x.Type == ClaimTypes.Sid).Value);
        }

        /// <summary>
        /// Sets the current user id.
        /// </summary>
        /// <param name="userId">The user id to set.</param>
        public void SetUserId(int userId)
        {
            AuthenticationManager.Value.SignOut();
            IssueAnonymousCookie(userId);
        }

        /// <summary>
        /// Logs in the current user as a new anonymous user.
        /// </summary>
        /// <returns>The new user id.</returns>
        public int LoginNewAnonymous()
        {
            var userId = CreateAnonymousUser();
            IssueAnonymousCookie(userId);
            return userId;
        }

        public bool Is(UserRole userRole)
        {
            throw new NotImplementedException();
            /*var serverConfiguration = AdministrationRepository.Value.GetServerConfiguration<ServerConfiguration>();

            switch (userRole)
            {
                case UserRole.Administrator:
                    return serverConfiguration.ServerRole == ServerRole.Administration;
                default:
                    throw new ArgumentOutOfRangeException("userRole");
            }*/
        }

        public bool IsValidAccessKey(string accessKey)
        {
            var validAccessKey = WebConfigurationManager.AppSettings["AdministrationAccessKey"];
            return accessKey == validAccessKey;
        }

        private static IEnumerable<Claim> ReadClaims()
        {
            UserClaims userClaims;

            if (!RequestCache.Current.TryGet(out userClaims))
            {
                userClaims = RequestCache.Current.Get(AuthenticationManager.Value.GetUserClaims);
            }
            
            return userClaims.Claims;
        }

        private static bool HasClaims(IEnumerable<Claim> claims)
        {
            return claims.Any(x => x.Type == ClaimTypes.Sid);
        }

        private static int CreateAnonymousUser()
        {
            return UserRepository.Value.CreateAnonymousUser();
        }

        private static void IssueAnonymousCookie(int userId)
        {
            var claims = new List<Claim>
                             {
                                 new Claim(ClaimTypes.Anonymous, ClaimTypes.Anonymous),
                                 new Claim(ClaimTypes.Sid, userId.ToString(CultureInfo.InvariantCulture))
                             };

            var userClaims = new UserClaims(claims);

            AuthenticationManager.Value.SignIn(userClaims);

            RequestCache.Current.Get(() => userClaims, true);
        }

        private static bool IsAnonymousCookie(IEnumerable<Claim> claims)
        {
            return claims.All(x => x.Type != ClaimTypes.Anonymous);
        }

        private static bool IsSecureAction()
        {
            // TODO: treat this as NotImplementedException
            return false;
        }

        private static bool IsSecurityStampValid(IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        private static void RedirectToLogin()
        {
            throw new NotImplementedException();
        }
    }
}
