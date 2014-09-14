using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;

using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;
using Disposable.Web.Caching;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

using Owin;

namespace Dilemma.Security
{
    internal class SecurityManager : ISecurityManager
    {
        private static readonly Lazy<IUserRepository> UserRepository = new Lazy<IUserRepository>(Locator.Current.Instance<IUserRepository>);

        public void ConfigureCookieAuthentication(IAppBuilder appBuilder)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                // NOTE: if this is not included then no redirects will take place.
                //LoginPath = new PathString("/Account/Login"),
                // TODO: change to always in production
                CookieSecure = CookieSecureOption.SameAsRequest,
                // 60 days
                ExpireTimeSpan = new TimeSpan(60, 0, 0, 0, 0),
                SlidingExpiration = true
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Sid;
        }

        public void CookieValidation()
        {
            var claims = ReadClaims().ToList();
            
            if (!HasCookie(claims))
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

        public int GetUserId()
        {
            return Int32.Parse(ReadClaims().Single(x => x.Type == ClaimTypes.Sid).Value);
        }

        public void SetUserId(int userId)
        {
            HttpContext.Current.Request.GetOwinContext().Authentication.SignOut();
            IssueAnonymousCookie(userId);
        }

        public int LoginNewAnonymous()
        {
            var userId = CreateAnonymousUser();
            IssueAnonymousCookie(userId);
            return userId;
        }

        private static IEnumerable<Claim> ReadClaims()
        {
            UserClaims userClaims;

            if (!RequestCache.Current.TryGet(out userClaims))
            {
                userClaims = RequestCache.Current.Get(
                    () =>
                        {
                            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
                            return new UserClaims(identity.Claims);
                        });
            }
            
            return userClaims.Claims;
        }

        private static bool HasCookie(IEnumerable<Claim> claims)
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

            var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = HttpContext.Current.Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;

            var authenticationProperties = new AuthenticationProperties { IsPersistent = true };

            authenticationManager.SignIn(authenticationProperties, id);

            RequestCache.Current.Get(() => new UserClaims(claims), true);
        }

        private static bool IsAnonymousCookie(IEnumerable<Claim> claims)
        {
            return claims.All(x => x.Type != ClaimTypes.Anonymous);
        }

        private static bool IsSecureAction()
        {
            return false;
            // TODO: treat this as NotImplementedException
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
