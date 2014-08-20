using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var model = new QuestionViewModel { QuestionId = id };
            return View(model);
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
