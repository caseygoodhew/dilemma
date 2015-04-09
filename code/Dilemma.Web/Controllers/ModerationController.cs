﻿using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    //[AllowUserRole(UserRole.Moderator)]
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

        [Route("moderation/question/{questionId:int:min(1)}")]
        public ActionResult QuestionHistory(int questionId)
        {
            var viewModel = ManualModerationService.Value.GetQuestionHistory(questionId);
            return View(viewModel);
        }
        
        [Route("moderation/answer/{answerId:int:min(1)}")]
        public ActionResult AnswerHistory(int answerId)
        {
            var viewModel = ManualModerationService.Value.GetAnswerHistory(answerId);
            return View(viewModel);
        }

        [Route("moderation/followup/{followupId:int:min(1)}")]
        public ActionResult FollowupHistory(int followupId)
        {
            var viewModel = ManualModerationService.Value.GetFollowupHistory(followupId);
            return View(viewModel);
        }
    }
}