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
            FilterAccessByEnum = new FilterAccessBySystemEnvironment(
                allowDeny,
                MapToSystemEnvironment(allowDeny, userRoles).Cast<object>());
        }

        protected FilterAccessByUserRoleAttribute(
            string controller,
            string action,
            AllowDeny allowDeny,
            params UserRole[] userRoles)
        {
            FilterAccessByEnum = new FilterAccessBySystemEnvironment(
                controller,
                action,
                allowDeny,
                MapToSystemEnvironment(allowDeny, userRoles).Cast<object>());
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FilterAccessByEnum.OnActionExecuting<FilterAccessByUserRoleAttribute>(filterContext);
        }

        private static IEnumerable<SystemEnvironment> MapToSystemEnvironment(AllowDeny allowDeny, IEnumerable<UserRole> userRoles)
        {
            return userRoles.SelectMany(userRole => RoleMap.Instance.Get(userRole, allowDeny));
        }
    }
}