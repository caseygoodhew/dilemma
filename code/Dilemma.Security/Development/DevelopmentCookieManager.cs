using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Security.Development
{
    /// <summary>
    /// Development Cookie Manager
    /// </summary>
    public static class DevelopmentCookieManager
    {
        private const string CookieName = "Dilemma.Security.Development.Cookie";

        private const string UserIds = "userids";

        private static readonly Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();
       
        /// <summary>
        /// Gets the user ids in the user's development cookie and ensures that the current user id 
        /// is added to the cookie if we're not running in a production environment.
        /// </summary>
        /// <returns>The list of user ids in the development cookie.</returns>
        public static IEnumerable<int> GetUserIds()
        {
            var myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);

            var userIds = ExtractUserIds(myCookie.Values[UserIds]).Concat(new[] { SecurityManager.Value.GetUserId() }).Distinct().ToList();

            if (IsInternalEnvironment())
            {
                myCookie.Values[UserIds] = userIds.Concat(",");
                myCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }

            return userIds;
        }

        private static bool IsInternalEnvironment()
        {
            return AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>().IsInternalEnvironment;
        }

        private static IEnumerable<int> ExtractUserIds(string userIds)
        {
            return string.IsNullOrEmpty(userIds)
                       ? Enumerable.Empty<int>()
                       : userIds.Split(',').Select(x => Convert.ToInt32(x)).Distinct();
        }
    }
}
