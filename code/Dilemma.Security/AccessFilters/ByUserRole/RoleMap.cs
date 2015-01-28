using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

using Disposable.Common.Extensions;

namespace Dilemma.Security.AccessFilters.ByUserRole
{
    internal class RoleMap
    {
        private static RoleMap instance;

        public static RoleMap Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoleMap();
                }

                return instance;
            }
        }

        private Dictionary<UserRole, Dictionary<AllowDeny, IEnumerable<SystemEnvironment>>> lookup =
            new Dictionary<UserRole, Dictionary<AllowDeny, IEnumerable<SystemEnvironment>>>();
        
        private RoleMap()
        {
            EnumExtensions.All<UserRole>().ToList().ForEach(
                userRole =>
                    {
                        switch (userRole)
                        {
                            case UserRole.Administrator:
                                Set(userRole, AllowDeny.Allow, SystemEnvironment.Administration, SystemEnvironment.Development);
                                Set(userRole, AllowDeny.Deny, SystemEnvironment.Administration);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("userRole");
                        }
                    });
        }
        
        public IEnumerable<SystemEnvironment> Get(UserRole userRole, AllowDeny allowDeny)
        {
            return lookup[userRole][allowDeny];
        }

        private void Set(UserRole userRole, AllowDeny allowDeny, params SystemEnvironment[] systemEnvironments)
        {
            if (!lookup.ContainsKey(userRole))
            {
                lookup[userRole] = new Dictionary<AllowDeny, IEnumerable<SystemEnvironment>>();
            }

            var entry = lookup[userRole];

            if (entry.ContainsKey(allowDeny))
            {
                throw new InvalidOperationException("Key already exists");
            }

            entry[allowDeny] = systemEnvironments;
        }
    }
}