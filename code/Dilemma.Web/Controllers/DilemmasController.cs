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
        private static readonly Lazy<ISiteService> SiteService =
            Locator.Lazy<ISiteService>();

        
        public ActionResult Index()
        {
            return Index(string.Empty);
        }

        //
        // GET: /Dilemmas/
        [Route("dilemmas/{category:alpha}")]
        public ActionResult Index(string category)
        {
            ViewBag.Category = string.IsNullOrEmpty(category) ? "all" : category;
            
            return View(GetDilemmasViewModel());
        }

        //
        // GET: /Dilemmas/QuestionId
        [Route("dilemmas/{questionId:int:min(1)}")]
        public ActionResult Index(int questionId)
        {
            // Not implemented
            return View();
        }

        private static DilemmasViewModel GetDilemmasViewModel()
        {
            var questions = (GetQuestions() ?? Enumerable.Empty<QuestionViewModel>()).ToList();
            
            return new DilemmasViewModel
                                {
                                    DilemmasToAnswer = questions.Where(x => x.IsOpen).OrderBy(x => x.CreatedDateTime),
                                    DilemmasToVote = questions.Where(x => x.IsClosed).OrderBy(x => x.ClosedDateTime),
                                    SidebarViewModel = GetSidebarViewModel()
                                };
        }

        private static IEnumerable<CategoryViewModel> GetCategories()
        {
            var rand = new Random();

            return new[] { new CategoryViewModel
                               {
                                   CategoryId = rand.Next(100),
                                   Name = "Personal"
                               },
                           new CategoryViewModel
                               {
                                   CategoryId = rand.Next(100),
                                   Name = "Family"
                               },
                           new CategoryViewModel
                               {
                                   CategoryId = rand.Next(100),
                                   Name = "Relationships"
                               },
                           new CategoryViewModel
                               {
                                   CategoryId = rand.Next(100),
                                   Name = "Work"
                               },
                           new CategoryViewModel
                               {
                                   CategoryId = rand.Next(100),
                                   Name = "Sex"
                               },
                           new CategoryViewModel
                               {
                                   CategoryId = rand.Next(100),
                                   Name = "Teenage"
                               } };
        }

        private static SidebarViewModel GetSidebarViewModel()
        {
            return new SidebarViewModel
                       {
                           Notifications = GetNotifications(),
                           UserStatsViewModel = GetUserStatsVieWModel()
                       };
        }

        private static UserStatsViewModel GetUserStatsVieWModel()
        {
            var rand = new Random();

            var level = rand.Next(1, 5);
            
            return new UserStatsViewModel
                       {
                           AnswersPosted = rand.Next(40000),
                           BestAnswersAwarded = rand.Next(40),
                           DilemmasPosted = rand.Next(1000),
                           UnreadNotifications = rand.Next(40),
                           UpVotesAwarded = rand.Next(100),
                           UserRank = new UserRankViewModel
                                          {
                                              IconFileName = "user-guru-30.png",
                                              Id = rand.Next(100),
                                              Name = new [] { "Guru", "Newbie", "Contributor" }.RandomUsing(rand),
                                              Level = level,
                                              Percentage = rand.Next(0, 25) + (25*(level-1))
                                          }
                       };
        }

        private static IEnumerable<NotificationViewModel> GetNotifications()
        {
            var rand = new Random();
            
            return new[] { new NotificationViewModel
                               {
                                   NotificationId = 100,
                                   NotificationType = NotificationType.PointsAwarded,
                                   CreatedDateTime = DateTime.Now.AddMinutes(-500),
                                   PointsAwarded = rand.Next(20) * (rand.Next(1, 4) == 1 ? -1 : 1)
                               },
                           new NotificationViewModel
                               {
                                   NotificationId = 101,
                                   NotificationType = NotificationType.QuestionAnswered,
                                   CreatedDateTime = DateTime.Now.AddMinutes(-1000)
                               } };
        }

        private static IEnumerable<QuestionViewModel> GetQuestions()
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

            var questionText =
@"So I'm confused about my relationship. I've been dating this guy for about 2 months now, and I go back and forth on if he's truly that into me. To begin with, he's older than me. I'm 18, he's 22, so there's automatically a lot of judgement that comes along with that, which might have to do with some of my concerns.

Basically, he says he likes me, drives about a half hour both ways in order to see me, pays for everything when we go out, texts me almost daily, finds excuses to touch me, seems interested in learning about me, and came on a trip to Vermont with my family.

However, he never asks to Skype or call anymore, hasn't told his parents about me, doesn't want to hang out at my house if my parents are there, I'm the one who asks to hangout (however I usually ask pretty far in advance), and when I talked about being official, he avoided the conversation.

My friends and family all refer to him as my boyfriend, but I don't know. I recently had sex with him and it was amazing, but I worry that I shouldn't have because we aren't technically official. He's said he's not messing around, but I still worry because he doesn't want to flat out say we're boyfriend and girlfriend. Am I being silly in my insecurities?";

            return Enumerable.Repeat(string.Empty, rand.Next(20)).Select(x => factory(questionText));
        }
    }
}
