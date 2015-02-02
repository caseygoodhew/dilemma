using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.AccessFilters.ByUserRole
{
    public abstract class FilterAccessByUserRoleAttribute : ActionFilterAttribute, IFilterAccessByEnumWrapper
    {
        public IFilterAccessByEnum FilterAccessByEnum { get; private set; }
        
        protected FilterAccessByUserRoleAttribute(AllowDeny allowDeny, params UserRole[] userRoles)
        {
            FilterAccessByEnum = new FilterAccessByServerRole(
                allowDeny,
                MapToServerRole(allowDeny, userRoles).Cast<object>());
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FilterAccessByEnum.OnActionExecuting<FilterAccessByUserRoleAttribute>(filterContext);
        }

        private static IEnumerable<ServerRole> MapToServerRole(AllowDeny allowDeny, IEnumerable<UserRole> userRoles)
        {
            return userRoles.SelectMany(userRole => RoleMap.Instance.Get(userRole, allowDeny));
        }
    }
}