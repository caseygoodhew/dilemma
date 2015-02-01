using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security.AccessFilters
{
    public class UnlockableAccessFilterAttribute : ActionFilterAttribute
    {
        protected static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();
        
        private readonly string controller;

        private readonly string action;

        private readonly string unlockKey;

        private readonly bool unlockDevelopment;
        
        public UnlockableAccessFilterAttribute(string configurationKey, string controller, string action) : this(configurationKey, controller, action, true)
        {
        }

        protected UnlockableAccessFilterAttribute(
            string configurationKey,
            string controller,
            string action,
            bool unlockDevelopment)
        {
            this.controller = controller;
            this.action = action;
            unlockKey = WebConfigurationManager.AppSettings[configurationKey];

            if (string.IsNullOrEmpty(unlockKey))
            {
                throw new InvalidOperationException(string.Format(@"Could not find unlock key ""{0}""", configurationKey));
            }

            this.unlockDevelopment = unlockDevelopment;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (unlockDevelopment)
            {
                var systemEnvironment =
                    AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>().SystemEnvironment;

                if (systemEnvironment == SystemEnvironment.Development)
                {
                    return;
                }
            }

            if (Unlocker.HasKey(filterContext.HttpContext.Request, unlockKey))
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