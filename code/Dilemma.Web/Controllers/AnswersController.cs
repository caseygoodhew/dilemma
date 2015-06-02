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

            if (questionDetails != null)
            {
                var answers = questionDetails.QuestionViewModel.Answers;
                var otherAnswers = answers.Where(x => !x.IsStarVote && !x.IsPopularVote && !x.IsMyAnswer);

                questionDetails.QuestionViewModel.Answers = new List<AnswerViewModel>
                                                                {
                                                                    answers.SingleOrDefault(x => x.IsStarVote),
                                                                    answers.SingleOrDefault(x => x.IsPopularVote),
                                                                    answers.SingleOrDefault(x => x.IsMyAnswer)
                                                                }.Concat(
                                                                    otherAnswers.OrderByDescending(x => x.VoteCount))
                    .Where(x => x != null)
                    .Distinct()
                    .ToList();
            }

            return View(new DilemmaDetailsViewModel
                                {
                                    QuestionDetails = questionDetails,
                                    Sidebar = GetSidebarViewModel()
                                });
        }
    }
}