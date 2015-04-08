using System;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.DataTransferObjects;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class AjaxController : DilemmaBaseController
    {
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();
            
        [HttpPost]
        public JsonResult Vote(VoteDto vote)
        {
            QuestionService.Value.RegisterVote(vote.AnswerId);
            
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult AddBookmark(BookmarkDto bookmark)
        {
            QuestionService.Value.AddBookmark(bookmark.QuestionId);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult RemoveBookmark(BookmarkDto bookmark)
        {
            QuestionService.Value.RemoveBookmark(bookmark.QuestionId);

            return Json(new { success = true });
        }

        [HttpPost]
        [Route("Advise/{questionId:int:min(1)}")]
        public ActionResult Advise(int questionId, AnswerViewModel viewModel)
        {
            var answerId = QuestionService.Value.RequestAnswerSlot(questionId);

            if (answerId == null)
            {
                return PartialView("DisplayTemplates/AnswerSlotsFull");
            }

            if (ModelState.IsValid)
            {
                viewModel.AnswerId = answerId;
                if (!QuestionService.Value.CompleteAnswer(questionId, viewModel))
                {
                    return PartialView("DisplayTemplates/AnswerSlotsFull");
                }

                ViewBag.CanVote = false;
                
                return PartialView(
                    "DisplayTemplates/Answer",
                    QuestionService.Value.GetQuestion(questionId)
                        .QuestionViewModel.Answers.Single(x => x.IsMyAnswer));
            }

            QuestionService.Value.TouchAnswer(answerId.Value);
            ViewBag.AnswerIsActive = true;

            return PartialView(viewModel);
        }

        [HttpPost]
        [Route("Followup/{questionId:int:min(1)}")]
        public ActionResult Followup(int questionId, FollowupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                QuestionService.Value.AddFollowup(questionId, viewModel);
                
                return PartialView(
                    "DisplayTemplates/FollowupResponse",
                    QuestionService.Value.GetQuestion(questionId).QuestionViewModel.Followup);
            }

            ViewBag.FollowupIsActive = true;

            return PartialView(viewModel);
        }
        
        [HttpPost]
        public JsonResult Report(ReportDto report)
        {
            return Json(new { success = true, report.QuestionId, report.AnswerId }); 
        }
    }
}