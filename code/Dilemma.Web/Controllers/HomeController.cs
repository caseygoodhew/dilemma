using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dilemma.Web.Controllers
{
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