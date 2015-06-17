using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    //[AllowSystemEnvironment(SystemEnvironment.Development)]
    [AllowUserRole(UserRole.Moderator)]
    public class ModerationController : DilemmaBaseController
    {
        private static readonly Lazy<IManualModerationService> ManualModerationService = Locator.Lazy<IManualModerationService>();

        //
        // GET: /Moderation/
        public ActionResult Index()
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

        [AllowUserRole(UserRole.Public)]
        [Route("moderation/dilemma/{questionId:int:min(1)}")]
        public ActionResult QuestionHistory(int questionId)
        {
            var viewModel = ManualModerationService.Value.GetQuestionHistory(questionId);
            return View(new ModerationHistoryViewModel<QuestionModerationHistoryViewModel>(viewModel, GetSidebarViewModel()));
        }

        [AllowUserRole(UserRole.Public)]
        [Route("moderation/answer/{answerId:int:min(1)}")]
        public ActionResult AnswerHistory(int answerId)
        {
            var viewModel = ManualModerationService.Value.GetAnswerHistory(answerId);
            return View(new ModerationHistoryViewModel<AnswerModerationHistoryViewModel>(viewModel, GetSidebarViewModel()));
        }

        [AllowUserRole(UserRole.Public)]
        [Route("moderation/followup/{followupId:int:min(1)}")]
        public ActionResult FollowupHistory(int followupId)
        {
            var viewModel = ManualModerationService.Value.GetFollowupHistory(followupId);
            return View(new ModerationHistoryViewModel<FollowupModerationHistoryViewModel>(viewModel, GetSidebarViewModel()));
        }
    }
}