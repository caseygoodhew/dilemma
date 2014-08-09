using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.ViewModels;

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
            return View(model);
        }
    }
}
