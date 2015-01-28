using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;
using Dilemma.Security.AccessFilters.ByEnum;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security.AccessFilters
{
    public abstract class UnlockableAccessFilterAttribute : ActionFilterAttribute
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();

        private readonly AllowDeny allowDeny;
        
        private readonly ServerRole serverRole;
        
        private readonly string controller;

        private readonly string action;

        private readonly string unlockKey;
        
        protected UnlockableAccessFilterAttribute(AllowDeny allowDeny, ServerRole serverRole, string controller, string action, string configurationKey)
        {
            if (allowDeny != AllowDeny.Allow && allowDeny != AllowDeny.Deny)
            {
                throw new ArgumentException("Only Allow or Deny can be used.");
            }
            
            this.allowDeny = allowDeny;
            this.serverRole = serverRole;
            this.controller = controller;
            this.action = action;
            unlockKey = WebConfigurationManager.AppSettings[configurationKey];
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentServerRole = AdministrationRepository.Value.GetServerConfiguration<ServerConfiguration>().ServerRole;

            if (Unlocker.HasKey(filterContext.HttpContext.Request, unlockKey))
            {
                return;
            }

            if (allowDeny == AllowDeny.Allow && currentServerRole == serverRole)
            {
                return;
            }

            if (allowDeny == AllowDeny.Deny && currentServerRole != serverRole)
            {
                return;
            }

            var currentController = filterContext.RouteData.Values["controller"].ToString();
            var currentAction = filterContext.RouteData.Values["action"].ToString();

            if (currentController != controller || currentAction != action)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                            {
                                controller,
                                action
                            }));
            }
        }


    }
}