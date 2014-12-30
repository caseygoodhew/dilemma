using Owin;

namespace Dilemma.Security
{
    internal interface IAuthenticationManager
    {
        /// <summary>
        /// Configures authentication in the OWIN pipeline.
        /// </summary>
        /// <param name="appBuilder">The OWIN app builder.</param>
        void Configure(IAppBuilder appBuilder);

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        void SignOut();

        /// <summary>
        /// Signs a user in with the provided claims.
        /// </summary>
        /// <param name="claims">The claims to support the user's identity.</param>
        void SignIn(UserClaims claims);

        /// <summary>
        /// Gets the <see cref="UserClaims"/> for the current user.
        /// </summary>
        /// <returns>The <see cref="UserClaims"/>.</returns>
        UserClaims GetUserClaims();
    }
}