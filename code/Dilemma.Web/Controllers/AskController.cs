using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class AskController : DilemmaBaseController
    {
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();
        
        //
        // GET: /Ask/
        public ActionResult Index()
        {
            var model = QuestionService.Value.InitNewQuestion();
            return View(model);
        }

        //
        // POST: /Ask/Create
        [HttpPost]
        public ActionResult Index(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionService.Value.SaveNewQuestion(model);
                return RedirectToAction("Index", "Dilemmas");
            }

            QuestionService.Value.InitNewQuestion(model);

            return View(model);
        }
    }
}
