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
        
        //
        // GET: /Question/
        /*public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Question/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }*/

        public ActionResult List()
        {
            return View(QuestionService.Value.GetAll());
        }

        //
        // GET: /Question/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Question/Create
        [HttpPost]
        public ActionResult Create(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionService.Value.SaveNew(model);
            }
            return View(model);
        }
    }
}
