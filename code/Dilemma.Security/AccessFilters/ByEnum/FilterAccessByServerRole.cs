using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;
using Dilemma.Security.AccessFilters.ByUserRole;

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

        protected override void AnnounceAllow(ActionExecutingContext filterContext)
        {
            // do nothing
        }

        protected override void AnnounceDeny(ActionExecutingContext filterContext, FilterAccessByEnum<ServerRole> accessFilter)
        {
            var controllerAction = ServerRoleControllerAction.GetControllerAction(GetComparisonValue());
            
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                        {
                            controller = controllerAction.Controller,
                            action = controllerAction.Action
                        }));
        }

        protected override ServerRole GetComparisonValue()
        {
            return AdministrationRepository.Value.GetServerConfiguration<ServerConfiguration>().ServerRole;
        }
    }
}