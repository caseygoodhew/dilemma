using System;
using System.Web.Mvc;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security
{
    /// <summary>
    /// Provides cookie based authentication services.
    /// </summary>
    public class DilemmaAuthenticationAttribute : ActionFilterAttribute
    {
        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SecurityManager.Value.ValidateClaims();
            base.OnActionExecuting(filterContext);
        }
    }
}
