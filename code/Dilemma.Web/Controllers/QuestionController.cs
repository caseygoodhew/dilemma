using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.Conversion;

namespace Dilemma.Web.Controllers
{
    public class QuestionController : Controller
    {
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
            return View(new QuestionRepository().List().Select(ConverterFactory.Convert<Question, QuestionViewModel>));
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
                var question = ConverterFactory.Convert<QuestionViewModel, Question>(model);
                new QuestionRepository().Create(question);
            }
            return View(model);
        }
    }
}
