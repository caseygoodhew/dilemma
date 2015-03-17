using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.ViewModels;

namespace Dilemma.Web.Controllers
{
    public class AskController : Controller
    {
        //
        // GET: /Ask/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Ask/Create
        [HttpPost]
        public ActionResult Index(CreateQuestionViewModel question)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
