using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.Extensions;
using Dilemma.Web.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class AskController : DilemmaBaseController
    {
        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();
        
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();
        
        [Route("ask")]
        public ActionResult Index()
        {
            return Index(string.Empty);
        }

        //
        // GET: /Dilemmas/
        [Route("ask/{category:alpha}")]
        public ActionResult Index(string category)
        {
            ViewBag.Category = category;
            return View(InitNewQuestion(CategoryHelper.GetCategory(category)));
        }

        //
        // POST: /Ask/Create
        [HttpPost]
        [ValidateInput(false)]
        [Route("ask")]
        public ActionResult Index(AskViewModel model)
        {
            return Index(string.Empty, model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("ask/{category:alpha}")]
        public ActionResult Index(string category, AskViewModel model)
        {
            ViewBag.Category = category;
            
            if (ModelState.IsValid)
            {
                var questionId = QuestionService.Value.SaveNewQuestion(model.Question);
				var questionDetails = QuestionService.Value.GetQuestion(questionId);

				return View("Confirm", new DilemmaDetailsViewModel {
					QuestionDetails = questionDetails,
					Sidebar = GetSidebarViewModel()
				});
            }

            return View(InitNewQuestion(model));
        }

        private static AskViewModel InitNewQuestion(CategoryViewModel category)
        {
            var questionViewModel = new QuestionViewModel();

            if (category != null)
            {
                questionViewModel.CategoryId = category.CategoryId;
                questionViewModel.CategoryName = category.Name;
            }
            
            return InitNewQuestion(new AskViewModel
            {
                Question = questionViewModel
            });
        }

        private static AskViewModel InitNewQuestion(AskViewModel questionViewModel)
        {
            if (questionViewModel == null)
            {
                questionViewModel = new AskViewModel
                {
                    Question = new QuestionViewModel()
                };
            }

            questionViewModel.Categories = SiteService.Value.GetCategories();
            questionViewModel.Sidebar = GetSidebarViewModel();

            return questionViewModel;
        }
    }
}
