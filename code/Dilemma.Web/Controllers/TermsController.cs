using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dilemma.Web.Controllers
{
    public class TermsController : Controller
    {
        [Route("cookies")]
        public ActionResult Cookies()
        {
            return View();
        }
	}
}