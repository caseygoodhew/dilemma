using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Security.AccessFilters;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return RedirectToRoute(new RouteValueDictionary(
                    new
                    {
                        controller = "Dilemmas",
                        action = "Index"
                    }));
        }
	}
}