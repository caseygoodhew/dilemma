using Owin;

namespace Dilemma.Security
{
    public interface ISecurityManager
    {
        void ConfigureCookieAuthentication(IAppBuilder appBuilder);
        
        void CookieValidation();

        int GetUserId();

        void SetUserId(int userId);

        int LoginNewAnonymous();
    }
}
