using System;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.ViewModels;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class ProfileController : DilemmaBaseController
    {
        //
        // GET: /Profile/
        public ActionResult Index()
        {
            var sidebar = GetSidebarViewModel();
            
            return View(new MyProfileViewModel
                            {
                                Sidebar = sidebar,
                                Notifications = sidebar.Notifications,
                                Dilemmas = Enumerable.Empty<QuestionViewModel>(),
                                Answers = Enumerable.Empty<QuestionViewModel>()
                            });
        }
    }
}