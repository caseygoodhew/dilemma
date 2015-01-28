using Dilemma.Common;

using Owin;

namespace Dilemma.Security
{
    /// <summary>
    /// Security manager interface.
    /// </summary>
    public interface ISecurityManager
    {
        /// <summary>
        /// Configures authentication in the OWIN pipeline using the configured IAuthenticationManager.
        /// </summary>
        /// <param name="appBuilder">The OWIN app builder.</param>
        void ConfigureAuthentication(IAppBuilder appBuilder);
        
        /// <summary>
        /// Validates the users security credentials.
        /// </summary>
        void ValidateClaims();

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

        /// <summary>
        /// Checks if the current user has the specified <see cref="UserRole"/>.
        /// </summary>
        /// <param name="userRole">The <see cref="UserRole"/> to check for.</param>
        /// <returns>Yes or no</returns>
        bool Is(UserRole userRole);

        bool IsValidAccessKey(string accessKey);
    }
}
