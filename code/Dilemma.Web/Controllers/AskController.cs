using System;
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
    public class AskController : DilemmaBaseController
    {
        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();
        
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();
        
        //
        // GET: /Ask/
        public ActionResult Index()
        {
            return View(InitNewQuestion());
        }

        //
        // POST: /Ask/Create
        [HttpPost]
        public ActionResult Index(AskViewModel model)
        {
            if (ModelState.IsValid)
            {
                QuestionService.Value.SaveNewQuestion(model.Question);
                return RedirectToAction("Index", "Dilemmas");
            }

            return View(InitNewQuestion(model));
        }

        private static AskViewModel InitNewQuestion(AskViewModel questionViewModel = null)
        {
            if (questionViewModel == null)
            {
                questionViewModel = new AskViewModel
                {
                    Question = new QuestionViewModel()
                };
            }

            questionViewModel.Categories = SiteService.Value.GetCategories();
            questionViewModel.Sidebar = XTestData.GetSidebarViewModel();

            return questionViewModel;
        }
    }
}
