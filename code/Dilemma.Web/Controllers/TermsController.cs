using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Security.AccessFilters;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class TermsController : Controller
    {
        [Route("cookies")]
        public ActionResult Cookies()
        {
            return View();
        }
	}
}