using System;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class ProfileController : DilemmaBaseController
    {
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();
        
        //
        // GET: /Profile/
        public ActionResult Index()
        {
            var questions = QuestionService.Value.GetMyActivity().ToList();
            var sidebar = GetSidebarViewModel();
            
            return View(new MyProfileViewModel
                            {
                                Sidebar = sidebar,
                                Notifications = sidebar.Notifications,
                                Dilemmas = questions.Where(x => x.IsMyQuestion),
                                Answers = questions.Where(x => !x.IsMyQuestion)
                            });
        }
    }
}