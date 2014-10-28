using Owin;

namespace Dilemma.Security
{
    /// <summary>
    /// Security manager interface.
    /// </summary>
    public interface ISecurityManager
    {
        /// <summary>
        /// Configures cookie based authentication in the OWIN pipeline.
        /// </summary>
        /// <param name="appBuilder">The OWIN app builder.</param>
        void ConfigureCookieAuthentication(IAppBuilder appBuilder);
        
        /// <summary>
        /// Validates the users security credentials.
        /// </summary>
        void ValidateCookie();

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <returns>The current user id.</returns>
        int GetUserId();

        /// <summary>
        /// Sets the current user id.
        /// </summary>
        /// <param name="userId">The user id to set.</param>
        void SetUserId(int userId);

        /// <summary>
        /// Logs in the current user as a new anonymous user.
        /// </summary>
        /// <returns>The new user id.</returns>
        int LoginNewAnonymous();
    }
}
