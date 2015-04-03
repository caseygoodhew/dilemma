using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.Extensions;
using Dilemma.Web.ViewModels;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class DilemmasController : DilemmaBaseController
    {
        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();

        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();

        [Route("dilemmas")]
        public ActionResult Index()
        {
            return Index(string.Empty);
        }

        //
        // GET: /Dilemmas/
        [Route("dilemmas/{category:alpha}")]
        public ActionResult Index(string category)
        {
            ICollection<QuestionViewModel> questions;

            category = CategoryHelper.ValidateCategory(category);
            
            if (category.Equals("bookmarks", StringComparison.CurrentCultureIgnoreCase))
            {
                questions = QuestionService.Value.GetBookmarkedQuestions().ToList();
            }
            else
            {
                var selectedCategory = CategoryHelper.GetCategory(category);

                questions = (selectedCategory == null
                                 ? QuestionService.Value.GetAllQuestions().ToList()
                                 : QuestionService.Value.GetQuestions(selectedCategory).ToList());
            }

            ViewBag.Category = category;

            return View(
                new DilemmasViewModel
                    {
                        DilemmasToAnswer = questions.Where(x => x.IsOpen).OrderByDescending(x => x.CreatedDateTime),
                        DilemmasToVote = questions.Where(x => x.IsClosed).OrderByDescending(x => x.ClosedDateTime),
                        Sidebar = GetSidebarViewModel()
                    });
        }
    }
}
