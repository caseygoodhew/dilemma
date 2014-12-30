using System;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

using Owin;

namespace Dilemma.Security
{
    internal class CookieAuthenticationManager : IAuthenticationManager
    {
        /// <summary>
        /// Configures cookie based authentication in the OWIN pipeline.
        /// </summary>
        /// <param name="appBuilder">The OWIN app builder.</param>
        public void Configure(IAppBuilder appBuilder)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
                                                   {
                                                       AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,

                                                       // NOTE: if this is not included then no redirects will take place.
                                                       // LoginPath = new PathString("/Account/Login"),
                                                       // TODO: change to always in production
                                                       CookieSecure = CookieSecureOption.SameAsRequest,

                                                       // 60 days
                                                       ExpireTimeSpan = new TimeSpan(60, 0, 0, 0, 0),
                                                       SlidingExpiration = true
                                                   });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Sid;
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        public void SignOut()
        {
            HttpContext.Current.Request.GetOwinContext().Authentication.SignOut();
        }

        /// <summary>
        /// Signs a user in with the provided claims.
        /// </summary>
        /// <param name="claims">The claims to support the user's identity.</param>
        public void SignIn(UserClaims claims)
        {
            var id = new ClaimsIdentity(claims.Claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = HttpContext.Current.Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;

            var authenticationProperties = new AuthenticationProperties { IsPersistent = true };

            authenticationManager.SignIn(authenticationProperties, id);
        }

        /// <summary>
        /// Gets the <see cref="UserClaims"/> for the current user.
        /// </summary>
        /// <returns>The <see cref="UserClaims"/>.</returns>
        public UserClaims GetUserClaims()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            return new UserClaims(identity.Claims);
        }
    }
}