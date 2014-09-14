using System;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security
{
    public class DenyProductionAccessAttribute : ActionFilterAttribute
    {
        private readonly static Lazy<IAdministrationRepository> AdministrationRepository = new Lazy<IAdministrationRepository>(Locator.Current.Instance<IAdministrationRepository>);

        private readonly string controller = "Home";

        private readonly string action = "Index";
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var systemConfiguration = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();

            if (systemConfiguration.SystemEnvironment != SystemEnvironment.Development && systemConfiguration.SystemEnvironment != SystemEnvironment.Testing)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller, action }));
            }
        }
        
    }
}
