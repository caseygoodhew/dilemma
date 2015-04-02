using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class AnswersController : DilemmaBaseController
    {
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();
        
            //
        // GET: /advise/QuestionId
        [Route("view/{questionId:int:min(1)}")]
        public ActionResult Index(int questionId)
        {
            var questionDetails = QuestionService.Value.GetQuestion(questionId);

            var answers = questionDetails.QuestionViewModel.Answers;
            var otherAnswers = answers.Where(x => !x.IsStarVote && !x.IsPopularVote && !x.IsMyAnswer);

            questionDetails.QuestionViewModel.Answers = new List<AnswerViewModel>
                                     {
                                         answers.SingleOrDefault(x => x.IsStarVote),
                                         answers.SingleOrDefault(x => x.IsPopularVote),
                                         answers.SingleOrDefault(x => x.IsMyAnswer)
                                     }.Concat(otherAnswers.OrderByDescending(x => x.VoteCount)).Where(x => x != null).Distinct().ToList();

            return View(new DilemmaDetailsViewModel
                                {
                                    QuestionDetails = questionDetails,
                                    Sidebar = XTestData.GetSidebarViewModel()
                                });
        }

        [Route("advise/{questionId:int:min(1)}")]
        public ActionResult Answer(int questionId)
        {
            var answerId = QuestionService.Value.RequestAnswerSlot(questionId);

            var viewModel = new DilemmaDetailsViewModel
                                {
                                    QuestionDetails = QuestionService.Value.GetQuestion(questionId),
                                    Sidebar = XTestData.GetSidebarViewModel()
                                };
            
            if (answerId.HasValue)
            {
                viewModel.MyAnswer = QuestionService.Value.GetAnswerInProgress(questionId, answerId.Value);
            }
            
            return View(viewModel);
        }

        [Route("advise/{questionId:int:min(1)}")]
        [HttpPost]
        public ActionResult Answer(int questionId, DilemmaDetailsViewModel viewModel)
        {
            var answerId = QuestionService.Value.RequestAnswerSlot(questionId);
            
            if (ModelState.IsValid)
            {
                viewModel.MyAnswer.AnswerId = answerId;
                QuestionService.Value.CompleteAnswer(questionId, viewModel.MyAnswer);
                return RedirectToAction("Index", "Answers", new { questionId });
            }

            var refreshedViewModel = new DilemmaDetailsViewModel
            {
                QuestionDetails = QuestionService.Value.GetQuestion(questionId),
                MyAnswer = viewModel.MyAnswer,
                Sidebar = XTestData.GetSidebarViewModel()
            };

            return View(refreshedViewModel);
        }
    }
}