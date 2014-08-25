using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.Conversion;
using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class QuestionController : Controller
    {
        private static readonly Lazy<IQuestionService> QuestionService = new Lazy<IQuestionService>(Locator.Current.Instance<IQuestionService>);

        public ActionResult List()
        {
            return View(QuestionService.Value.GetAll());
        }

        //
        // GET: /Question/Create
        public ActionResult Create()
        {
            var model = QuestionService.Value.InitNew();
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
        //
        // POST: /Question/Create
        [HttpPost]
        public ActionResult Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionService.Value.SaveNew(model);
                return RedirectToAction("List");
            }

            QuestionService.Value.InitNew(model);
            
            return View(model);
        }
    }
}
