using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class ActivityController : Controller
    {
        private static readonly Lazy<IQuestionService> QuestionService = new Lazy<IQuestionService>(Locator.Current.Instance<IQuestionService>);

        //
        // GET: /Activity/
        public ActionResult Index()
        {
            var viewModel = new QuestionListViewModel { Questions = QuestionService.Value.GetMyActivity() };

            return View(viewModel);
        }
	}
}