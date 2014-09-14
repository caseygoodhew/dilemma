using System.Collections.Generic;
using System.Security.Claims;

namespace Dilemma.Security
{
    public class UserClaims
    {
        public readonly IEnumerable<Claim> Claims;
        
        public UserClaims(IEnumerable<Claim> claims)
        {
            Claims = claims;
        }
    }
}
