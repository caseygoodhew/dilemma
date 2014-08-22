using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.Conversion;
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
        public ActionResult Details(int id)
        {
            return View(QuestionService.Value.GetQuestion(id));
        }

        //
        // GET: /Question/AnswerSlot/id
        public ActionResult AnswerSlot(int id)
        {
            return RedirectToAction("Answer", new { questionId = id, answerId = QuestionService.Value.RequestAnswerSlot(id) });
        }

        [Route("question/answer/{questionId:int:min(1)}/{answerId:int:min(1)}")]
        public ActionResult Answer(int questionId, int answerId)
        {
            var viewModel = QuestionService.Value.GetQuestion(questionId);
            viewModel.Answer = QuestionService.Value.GetAnswerInProgress(questionId, answerId);
            
            return View(viewModel);
        }

        [Route("question/answer/{questionId:int:min(1)}/{answerId:int:min(1)}")]
        [HttpPost]
        public ActionResult Answer(int questionId, int answerId, QuestionDetailsViewModel viewModel)
        {
            throw new NotImplementedException();
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
