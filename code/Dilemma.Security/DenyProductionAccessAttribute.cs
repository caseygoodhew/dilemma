using System;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security
{
    /// <summary>
    /// Security attribute to deny access to a controller or action in a production environment by redirecting to the home page.
    /// </summary>
    [Obsolete]
    public class DenyProductionAccessAttribute : ActionFilterAttribute
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();

        private readonly string controller = "Home";

        private readonly string action = "Index";
        
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var systemConfiguration = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();

            if (!systemConfiguration.IsInternalEnvironment)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller, action }));
            }
        }
    }
}
