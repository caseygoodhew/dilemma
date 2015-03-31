using System;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Security.AccessFilters;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class ProfileController : DilemmaBaseController
    {
        //
        // GET: /Profile/
        public ActionResult Index()
        {
            return View(XTestData.GetMyProfileViewModel());
        }
    }
}