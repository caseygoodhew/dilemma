using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class ActivityController : Controller
    {
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();

        //
        // GET: /Activity/
        public ActionResult Index()
        {
            var viewModel = new QuestionListViewModel { Questions = QuestionService.Value.GetMyActivity() };

            return View(viewModel);
        }
	}
}