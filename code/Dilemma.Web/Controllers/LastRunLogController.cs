using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Administrator)]
    public class LastRunLogController : Controller
    {
        private static readonly Lazy<IAdministrationService> AdministrationService =
            Locator.Lazy<IAdministrationService>();
        
        //
        // GET: /LastRunLog/
        public ActionResult Index()
        {
            return View(AdministrationService.Value.GetLastRunLog());
        }

        [Route("ExpireAnswerSlots")]
        public ActionResult ExpireAnswerSlots()
        {
            AdministrationService.Value.ExpireAnswerSlots();
            return RedirectToAction("Index");
        }

        [Route("RetireOldQuestions")]
        public ActionResult RetireOldQuestions()
        {
            AdministrationService.Value.RetireOldQuestions();
            return RedirectToAction("Index");
        }

        [Route("CloseQuestions")]
        public ActionResult CloseQuestions()
        {
            AdministrationService.Value.CloseQuestions(); 
            return RedirectToAction("Index");
        }
	}
}