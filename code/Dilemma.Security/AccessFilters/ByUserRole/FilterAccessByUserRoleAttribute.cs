using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

using Disposable.Common.Extensions;

namespace Dilemma.Security.AccessFilters.ByUserRole
{
    public abstract class FilterAccessByUserRoleAttribute : ActionFilterAttribute, IFilterAccessByEnumWrapper
    {
        public IFilterAccessByEnum FilterAccessByEnum { get; private set; }
        
        private class RoleMap : Dictionary<UserRole, Dictionary<AllowDeny, IEnumerable<SystemEnvironment>>>
        {
            public void Set(UserRole userRole, AllowDeny allowDeny, params SystemEnvironment[] systemEnvironments)
            {
                if (!ContainsKey(userRole))
                {
                    this[userRole] = new Dictionary<AllowDeny, IEnumerable<SystemEnvironment>>();
                }

                var entry = this[userRole];

                if (entry.ContainsKey(allowDeny))
                {
                    throw new InvalidOperationException("Key already exists");
                }

                entry[allowDeny] = systemEnvironments;
            }
        }
        
        private static RoleMap roleMap;

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
            return userRoles.SelectMany(userRole => GetRoleMap()[userRole][allowDeny]);
        }

        private static RoleMap GetRoleMap()
        {
            if (roleMap == null)
            {
                roleMap = new RoleMap();

                EnumExtensions.All<UserRole>().ToList().ForEach(
                    userRole =>
                        {
                            switch (userRole)
                            {
                                case UserRole.Administrator:
                                    roleMap.Set(userRole, AllowDeny.Allow, SystemEnvironment.Administration, SystemEnvironment.Development);
                                    roleMap.Set(userRole, AllowDeny.Deny, SystemEnvironment.Administration);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException("userRole");
                            } 
                        });
            }

            return roleMap;
        }
    }
}