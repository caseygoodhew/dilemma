using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.Services;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class OfflineController : Controller
    {
        private static readonly Lazy<IAdministrationService> AdministrationService = Locator.Lazy<IAdministrationService>();
        
        //
        // GET: /Offline/
        public ActionResult Index()
        {
            if (AdministrationService.Value.IsOnline())
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }
    }
}