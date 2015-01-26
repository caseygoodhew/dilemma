using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Administrator)]
    public class ModerationController : Controller
    {
        private static readonly Lazy<IManualModerationService> ManualModerationService = Locator.Lazy<IManualModerationService>();
        
        //
        // GET: /Moderation/
        public ActionResult Index()
        {
            return View(ManualModerationService.Value.GetNext());
        }

        public ActionResult Details(int moderationId)
        {
            return View(ManualModerationService.Value.GetNext());
        }

        [HttpPost]
        public ActionResult Approve(ModerationViewModel viewModel)
        {
            ManualModerationService.Value.Approve(viewModel.ModerationId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reject(RejectModerationViewModel viewModel)
        {
            ManualModerationService.Value.Reject(viewModel.ModerationId, viewModel.Message);
            return RedirectToAction("Index");
        }
    }
}