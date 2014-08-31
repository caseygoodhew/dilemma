using System;
using System.Web.Mvc;

using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public abstract class DilemmaBaseController : Controller
    {
        private readonly static Lazy<ISecurityManager> SecurityManager = new Lazy<ISecurityManager>(Locator.Current.Instance<ISecurityManager>);
        
        protected DilemmaBaseController()
        {
            SecurityManager.Value.CookieValidation();
            // this will throw an exception if its the first time tat the user has been to the page (the cookie hasn't been sent back to the browser but the claim is being checked).
            SecurityManager.Value.GetUserId(); 
        }
    }
}