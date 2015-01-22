using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security.AccessFilters
{
    public class QuestionSeederAccessFilterAttribute : ActionFilterAttribute
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();

        private static readonly string Controller = "Question";

        private static readonly string Action = "Seeder";

        private static readonly string UnlockKey = "7c80d66d0b564c7790c584f47f9aff0c";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>().SystemEnvironment
                != SystemEnvironment.QuestionSeeder || HasUnlockKey(filterContext))
            {
                return;
            }

            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            if (controller != Controller || action != Action)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                            {
                                controller = Controller,
                                action = Action
                            }));
            }
        }

        private static bool HasUnlockKey(ActionExecutingContext filterContext)
        {
            if (HasUnlockKey(filterContext.HttpContext.Request.Params))
            {
                return true;
            }

            var referer = filterContext.HttpContext.Request.UrlReferrer;
            if (referer == null)
            {
                return false;
            }

            return HasUnlockKey(HttpUtility.ParseQueryString(referer.Query));
        }

        private static bool HasUnlockKey(NameValueCollection dict)
        {
            return StringComparer.InvariantCultureIgnoreCase.Compare(UnlockKey, dict["key"]) == 0;
        }
    }
}