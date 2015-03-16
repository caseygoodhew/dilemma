using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Web.ViewModels;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class DilemmasController : DilemmaBaseController
    {
        private static readonly Lazy<ISiteService> SiteService =
            Locator.Lazy<ISiteService>();

        
        public ActionResult Index()
        {
            return Index(string.Empty);
        }

        //
        // GET: /Dilemmas/
        public ActionResult Index(string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                throw new NotImplementedException();
            }
            
            ViewBag.CategoryNumber = "";
            ViewBag.CategoryName = "";
            
            var questions = (TMPGetQuestions() ?? Enumerable.Empty<QuestionViewModel>()).ToList();

            var viewModel = new DilemmasViewModel
                                {
                                    DilemmasToAnswer = questions.Where(x => x.IsOpen).OrderBy(x => x.CreatedDateTime),
                                    DilemmasToVote = questions.Where(x => x.IsClosed).OrderBy(x => x.ClosedDateTime)
                                };

            return View(viewModel);
        }

        //
        // GET: /Dilemmas/QuestionId
        public ActionResult Index(int questionId)
        {
            return View();
        }



        private static IEnumerable<QuestionViewModel> TMPGetQuestions()
        {
            var rand = new Random();
            var categories = SiteService.Value.GetCategories();
            var now = DateTime.Now;

            Func<string, QuestionViewModel> factory = (text) =>
                {
                    var category = categories.RandomUsing(rand);
                    var created = now.AddHours(-1*rand.Next(8 * 24));
                    var closes = created.AddDays(5);
                    var maxAnswers = 5;
                    var totalAnswers = rand.Next(0, maxAnswers);
                    var isClosed = totalAnswers == maxAnswers || closes < now;
                    
                    DateTime? closed = null;
                    if (isClosed)
                    {
                        closed = totalAnswers == maxAnswers ? created.AddMinutes(rand.Next(60 * 24)) : closes;
                    }

                    return new QuestionViewModel
                               {
                                   QuestionId = rand.Next(1000, 2000),
                                   CategoryId = category.CategoryId,
                                   CategoryName = category.Name,
                                   IsClosed = isClosed,
                                   IsOpen = !isClosed,
                                   Text = text,
                                   TotalAnswers = totalAnswers,
                                   CreatedDateTime = created,
                                   ClosesDateTime = closes,
                                   ClosedDateTime = closed,
                                   IsApproved = true,
                                   MaxAnswers = maxAnswers
                               };
                };

            return new[]
                    {
                        factory("this is a question"), 
                        factory("this is a question"), 
                        factory("this is a question"),
                        factory("this is a question"), 
                        factory("this is a question"), 
                        factory("this is a question"),
                        factory("this is a question"), 
                        factory("this is a question"), 
                        factory("this is a question"),
                        factory("this is a question"), 
                        factory("this is a question"), 
                        factory("this is a question"),
                        factory("this is a question"), 
                        factory("this is a question"), 
                        factory("this is a question"),
                        factory("this is a question"), 
                        factory("this is a question"), 
                        factory("this is a question"),
                        factory("this is a question"),
                    };
        }
    }
}
