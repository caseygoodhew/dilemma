using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dilemma.Web.Controllers
{
    public class PrelaunchViewModel
    {
        public string Question;

        public string FindMe;
    }

    public class PrelaunchController : DilemmaBaseController
    {
        //
        // GET: /Prelaunch/
        public ActionResult Index()
        {
            return View(new PrelaunchViewModel());
        }

        [HttpPost]
        public ActionResult Index(PrelaunchViewModel viewModel)
        {
            return View(new PrelaunchViewModel());
        }
	}
}