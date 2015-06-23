using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dilemma.Business.Services;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class HomeController : DilemmaBaseController
    {
        private static readonly Lazy<IAdministrationService> AdministrationService =
            Locator.Lazy<IAdministrationService>();
        
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var systemServerConfiguration = AdministrationService.Value.GetSystemServerConfiguration();

            ViewBag.SuppressPageBd = true;
            ViewBag.WeeksQuestionsOpen =
                NumberToText(Math.Ceiling(
                    ((decimal)
                        (systemServerConfiguration.SystemConfigurationViewModel.QuestionLifetimeDays +
                         systemServerConfiguration.SystemConfigurationViewModel.RetireQuestionAfterDays))/7));

            return View();
        }
	}
}