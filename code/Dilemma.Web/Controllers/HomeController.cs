﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dilemma.Web.Controllers
{
    public class HomeController : DilemmaBaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
            //return RedirectToAction("Create", "Question");
        }
	}
}