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
    public class StaticPagesController : DilemmaBaseController
    {
        [Route("about")]
        [Route("contact")]
        public ActionResult Index()
        {
            return View();
        }
        
        
        [Route("sitemap")]
        public ActionResult EmptyPage()
        {
            return View();
        }
	}
}