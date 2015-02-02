using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security.AccessFilters.ByEnum
{
    public class FilterAccessBySystemEnvironment : FilterAccessByEnum<SystemEnvironment>
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();

        public FilterAccessBySystemEnvironment(AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
            : base(allowDeny, comparisonEnums)
        {
        }

        protected override void AnnounceAllow(ActionExecutingContext filterContext)
        {
            // do nothing
        }

        protected override void AnnounceDeny(ActionExecutingContext filterContext, FilterAccessByEnum<SystemEnvironment> accessFilter)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                        {
                            controller = "Home",
                            action = "Index"
                        }));
        }

        protected override SystemEnvironment GetComparisonValue()
        {
            return AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>().SystemEnvironment;
        }
    }
}