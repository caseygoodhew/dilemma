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

        private readonly Dictionary<UserRole, Dictionary<AllowDeny, IEnumerable<ServerRole>>> lookup =
            new Dictionary<UserRole, Dictionary<AllowDeny, IEnumerable<ServerRole>>>();
        
        private RoleMap()
        {
            EnumExtensions.All<UserRole>().ToList().ForEach(
                userRole =>
                    {
                        Set(userRole, AllowDeny.Allow, ServerRole.Public, ServerRole.QuestionSeeder);
                        Set(userRole, AllowDeny.Deny, ServerRole.Offline);
                        
                        switch (userRole)
                        {
                            // for now, administrators and moderators are the same thing
                            case UserRole.Administrator:
                            case UserRole.Moderator:
                                Set(userRole, AllowDeny.Allow, ServerRole.Moderation);
                                Set(userRole, AllowDeny.Deny, ServerRole.Moderation);
                                break;
                            case UserRole.Public:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("userRole");
                        }
                    });
        }

        public IEnumerable<ServerRole> Get(UserRole userRole, AllowDeny allowDeny)
        {
            return lookup[userRole][allowDeny];
        }

        private void Set(UserRole userRole, AllowDeny allowDeny, params ServerRole[] serverRoles)
        {
            if (!lookup.ContainsKey(userRole))
            {
                lookup[userRole] = new Dictionary<AllowDeny, IEnumerable<ServerRole>>();
            }

            var entry = lookup[userRole];

            if (!entry.ContainsKey(allowDeny))
            {
                entry[allowDeny] = Enumerable.Empty<ServerRole>();
            }

            entry[allowDeny] = entry[allowDeny].Concat(serverRoles).Distinct();
        }
    }
}