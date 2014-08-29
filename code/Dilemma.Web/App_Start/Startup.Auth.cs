using System;
using System.Security.Cryptography.X509Certificates;

using Dilemma.Security;

using Disposable.Common.ServiceLocator;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Dilemma.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            Locator.Current.Instance<ISecurityManager>().ConfigureCookieAuthentication(app);
        }
    }
}