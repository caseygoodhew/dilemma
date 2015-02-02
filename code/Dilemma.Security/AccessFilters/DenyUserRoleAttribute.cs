using System;
using System.Linq;

using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;
using Dilemma.Security.AccessFilters.ByUserRole;

namespace Dilemma.Security.AccessFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyUserRoleAttribute : FilterAccessByUserRoleAttribute
    {
        public DenyUserRoleAttribute(UserRole userRole, params UserRole[] userRoles)
            : base(AllowDeny.Deny, new[] { userRole }.Concat(userRoles).ToArray())
        {
        }
    }
}