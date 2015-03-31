using System;
using System.Web.Mvc;

namespace Dilemma.Web.Controllers
{
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