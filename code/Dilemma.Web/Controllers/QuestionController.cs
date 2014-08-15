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

        private static readonly Lazy<ISiteService> SiteService = new Lazy<ISiteService>(Locator.Current.Instance<ISiteService>);
        
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
            var model = new CreateQuestionViewModel { Categories = SiteService.Value.GetCategories() };
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
            }

            model.Categories = SiteService.Value.GetCategories();

            return View(model);
        }
    }
}
