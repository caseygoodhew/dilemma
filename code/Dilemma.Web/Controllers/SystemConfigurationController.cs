using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class SystemConfigurationController : DilemmaBaseController
    {
        private static readonly Lazy<IAdministrationService> AdministrationService = new Lazy<IAdministrationService>(Locator.Current.Instance<IAdministrationService>);
        
        //
        // GET: /SystemConfiguration/
        public ActionResult Index()
        {
            return View(AdministrationService.Value.GetSystemConfiguration());
        }

        //
        // POST: /SystemConfiguration/
        [HttpPost]
        public ActionResult Index(SystemConfigurationViewModel systemConfiguration)
        {
            if (ModelState.IsValid)
            {
                AdministrationService.Value.SetSystemConfiguration(systemConfiguration);
                return RedirectToAction("Index");
            }

            return View(systemConfiguration);
        }
    }
}
