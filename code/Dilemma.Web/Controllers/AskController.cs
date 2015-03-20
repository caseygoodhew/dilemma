using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Web.ViewModels;

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
            var model = TestData.InitNewQuestion();
            return View(model);
        }

        //
        // POST: /Ask/Create
        [HttpPost]
        public ActionResult Index(AskViewModel model)
        {
            if (ModelState.IsValid)
            {
                //QuestionService.Value.SaveNewQuestion(model.Question);
                return RedirectToAction("Index", "Dilemmas");
            }

            TestData.InitNewQuestion(model);

            return View(model);
        }
    }
}
