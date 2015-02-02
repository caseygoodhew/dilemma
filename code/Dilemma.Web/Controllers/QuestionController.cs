using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security;
using Dilemma.Security.AccessFilters;

using Disposable.Common.Conversion;
using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class QuestionController : DilemmaBaseController
    {
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        public ActionResult List()
        {
            var viewModel = new QuestionListViewModel { Questions = QuestionService.Value.GetAllQuestions() };
            
            return View(viewModel);
        }

        //
        // GET: /Question/Create
        public ActionResult Create()
        {
            var model = QuestionService.Value.InitNewQuestion();
            return View(model);
        }

        //
        // POST: /Question/Create
        [HttpPost]
        public ActionResult Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionService.Value.SaveNewQuestion(model);
                return RedirectToAction("List");
            }

            QuestionService.Value.InitNewQuestion(model);
            
            return View(model);
        }

        //
        // GET: /Question/Seeder
        [DenyUserRole(UserRole.Public, UserRole.Moderator)]
        public ActionResult Seeder()
        {
            SecurityManager.Value.LoginNewAnonymous();
            return Create();
        }

        //
        // POST: /Question/Seeder
        [HttpPost]
        [DenyUserRole(UserRole.Public, UserRole.Moderator)]
        public ActionResult Seeder(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ShowConfirmation = true;
                QuestionService.Value.SaveNewQuestion(model);
                return View(QuestionService.Value.InitNewQuestion());
            }
            
            QuestionService.Value.InitNewQuestion(model);

            return View(model);
        }

        //
        // GET: /Question/Details
        [Route("question/{questionId:int:min(1)}")]
        public ActionResult Details(int questionId)
        {
            return View(QuestionService.Value.GetQuestion(questionId));
        }

        //
        // GET: /Question/AnswerSlot/id
        [Route("question/{questionId:int:min(1)}/answer")]
        public ActionResult AnswerSlot(int questionId)
        {
            var answerId = QuestionService.Value.RequestAnswerSlot(questionId);
            
            return RedirectToAction("Answer", new { questionId, answerId = answerId ?? 0 });
        }

        [Route("question/{questionId:int:min(1)}/answer/{answerId:int:min(0)}")]
        public ActionResult Answer(int questionId, int answerId)
        {
            var viewModel = QuestionService.Value.GetQuestion(questionId);
            
            if (answerId > 0)
            {
                viewModel.Answer = QuestionService.Value.GetAnswerInProgress(questionId, answerId);
            }

            return View(viewModel);
        }

        [Route("question/{questionId:int:min(1)}/answer/{answerId:int:min(1)}")]
        [HttpPost]
        public ActionResult Answer(int questionId, int answerId, QuestionDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Answer.AnswerId = answerId;
                QuestionService.Value.CompleteAnswer(questionId, viewModel.Answer);
                return RedirectToAction("Details", "Question", new { questionId });
            }

            var refreshedViewModel = QuestionService.Value.GetQuestion(questionId);
            refreshedViewModel.Answer = viewModel.Answer;
            viewModel.Answer.AnswerId = answerId;

            return View(refreshedViewModel);
        }
    }
}
