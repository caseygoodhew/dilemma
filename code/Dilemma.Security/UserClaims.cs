using System.Collections.Generic;
using System.Security.Claims;

namespace Dilemma.Security
{
    /// <summary>
    /// Security claims wrapper keep a collection of user claims.
    /// </summary>
    public class UserClaims
    {
        /// <summary>
        /// The security claims.
        /// </summary>
        public readonly IEnumerable<Claim> Claims;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaims"/> class.
        /// </summary>
        /// <param name="claims">The security claims.</param>
        public UserClaims(IEnumerable<Claim> claims)
        {
            Claims = claims;
        }
    }
}
