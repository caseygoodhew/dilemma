using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Security;
using Dilemma.Security.AccessFilters;

namespace Dilemma.Web.Controllers
{
    public class HomeController : DilemmaBaseController
    {
        //
        // GET: /Home/
        [DilemmaAuthentication]
        public ActionResult Index()
        {
            return View();
            //return RedirectToAction("Create", "Question");
        }
    }
}