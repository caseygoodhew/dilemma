using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class ModerationController : Controller
    {
        private static readonly Lazy<IModerationService> ModerationService = Locator.Lazy<IModerationService>();
        
        //
        // GET: /Moderation/
        public ActionResult Index()
        {
            return View(ModerationService.Value.GetNext());
        }

        public ActionResult Details(int moderationId)
        {
            return View(ModerationService.Value.GetNext());
        }

        [HttpPost]
        public ActionResult Approve(ModerationViewModel viewModel)
        {
            ModerationService.Value.Approve(viewModel.ModerationId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reject(RejectModerationViewModel viewModel)
        {
            ModerationService.Value.Reject(viewModel.ModerationId, viewModel.Message);
            return RedirectToAction("Index");
        }
    }
}