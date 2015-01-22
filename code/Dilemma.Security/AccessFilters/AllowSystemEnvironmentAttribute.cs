using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Security.AccessFilters.ByEnum;

using Disposable.Common.Extensions;

namespace Dilemma.Security.AccessFilters
{
    /// <summary>
    /// Security attribute to allow access to a controller or action based on the current <see cref="SystemEnvironment"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AllowSystemEnvironmentAttribute : FilterAccessByEnumWrapperAttribute
    {
        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should allow.
        /// </summary>
        /// <param name="systemEnvironment">The system environment to check for.</param>
        /// <param name="additionalSystemEnvironments">Additional system environments to check for.</param>
        public AllowSystemEnvironmentAttribute(SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(typeof(FilterAccessBySystemEnvironment), AllowDeny.Allow, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).Cast<object>())
        {
        }

        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should allow.
        /// </summary>
        /// <param name="controller">The controller that should be redirected to in the event of a failure.</param>
        /// <param name="action">The action that should be redirected to in the event of a failure.</param>
        /// <param name="systemEnvironment">The system environment to check for.</param>
        /// <param name="additionalSystemEnvironments">Additional system environments to check for.</param>
        public AllowSystemEnvironmentAttribute(string controller, string action, SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(typeof(FilterAccessBySystemEnvironment), controller, action, AllowDeny.Allow, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).Cast<object>())
        {
        }
    }

    public abstract class FilterAccessByUserRoleAttribute : ActionFilterAttribute
    {
        // working on this functionality!!!!
        rivate class RoleMap : Dictionary<UserRoles, Dictionary<AllowDeny, IEnumerable<SystemEnvironment>>>
        {
            public void Set(UserRoles userRole, AllowDeny allowDeny, params SystemEnvironment[] systemEnvironments)
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
        
        public readonly IFilterAccessByEnum FilterAccessByEnum;

        private static RoleMap roleMap;
        
        protected FilterAccessByUserRoleAttribute(AllowDeny allowDeny, params UserRoles[] userRoles)
        {
            FilterAccessByEnum = new FilterAccessBySystemEnvironment(allowDeny, MapToSystemEnvironment(allowDeny, userRoles).Cast<object>());
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FilterAccessByEnum.OnActionExecuting<FilterAccessByEnumWrapperAttribute>(filterContext);
        }

        private static IEnumerable<SystemEnvironment> MapToSystemEnvironment(AllowDeny allowDeny, IEnumerable<UserRoles> userRoles)
        {
            return userRoles.SelectMany(userRole => GetRoleMap()[userRole][allowDeny]);
        }

        private static RoleMap GetRoleMap()
        {
            if (roleMap == null)
            {
                roleMap = new RoleMap();

                EnumExtensions.All<UserRoles>().ToList().ForEach(
                    userRole =>
                        {
                            switch (userRole)
                            {
                                case UserRoles.Administrator:
                                    roleMap.Set(userRole, AllowDeny.Allow, SystemEnvironment.Administration, SystemEnvironment.Development);
                                    roleMap.Set(userRole, AllowDeny.Allow, SystemEnvironment.Administration);
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