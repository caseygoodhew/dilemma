using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Web.ViewModels;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
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
            var categories = SiteService.Value.GetCategories();

            var selectedCategory =
                categories.FirstOrDefault(
                    x => string.Equals(x.Name, category, StringComparison.CurrentCultureIgnoreCase));
            
            ViewBag.Category = selectedCategory == null ? "all" : selectedCategory.Name;

            var questions = QuestionService.Value.GetQuestions(selectedCategory);

            return View(
                new DilemmasViewModel
                    {
                        DilemmasToAnswer = questions.Where(x => x.IsOpen).OrderByDescending(x => x.CreatedDateTime),
                        DilemmasToVote = questions.Where(x => x.IsClosed).OrderByDescending(x => x.ClosedDateTime),
                        Sidebar = XTestData.GetSidebarViewModel()
                    });
        }
    }
}
