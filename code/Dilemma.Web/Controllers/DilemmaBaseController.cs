using System;
using System.Web;
using System.Web.Mvc;

using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public abstract class DilemmaBaseController : Controller
    {
        private readonly static Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();
        
        protected DilemmaBaseController()
        {
            // SecurityManager.Value.CookieValidation();
            
        }
    }
}