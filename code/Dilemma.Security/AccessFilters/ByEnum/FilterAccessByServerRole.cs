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
    public class FilterAccessByServerRole : FilterAccessByEnum<ServerRole>
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();
        
        public FilterAccessByServerRole(AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
            : base(allowDeny, comparisonEnums)
        {
        }

        public FilterAccessByServerRole(string controller, string action, AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
            : base(controller, action, allowDeny, comparisonEnums)
        {
        }

        protected override void AnnounceAllow(ActionExecutingContext filterContext)
        {
            // do nothing
        }

        protected override void AnnounceDeny(ActionExecutingContext filterContext, FilterAccessByEnum<ServerRole> accessFilter)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                        {
                            controller = accessFilter.Controller,
                            action = accessFilter.Action
                        }));
        }

        protected override ServerRole GetComparisonValue()
        {
            return AdministrationRepository.Value.GetServerConfiguration<ServerConfiguration>().ServerRole;
        }
    }
}