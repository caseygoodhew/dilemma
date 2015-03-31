using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security;
using Dilemma.Security.AccessFilters;
using Dilemma.Security.Development;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowSystemEnvironment(SystemEnvironment.Development)]
    [AllowUserRole(UserRole.Public)]
    public sealed class QuickAuthenticationController : DilemmaBaseController
    {
        private static readonly Lazy<IDevelopmentService> DevelopmentService = Locator.Lazy<IDevelopmentService>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        [Route("QuickAuthentication")]
        public ActionResult Index()
        {
            var userIds = DevelopmentCookieManager.GetUserIds();

            var viewModel = DevelopmentService.Value.GetList(userIds).ToList();

            var currentUserId = SecurityManager.Value.GetUserId();

            var currentDevelopmentUser = viewModel.FirstOrDefault(x => x.UserId == currentUserId);

            if (currentDevelopmentUser != null)
            {
                currentDevelopmentUser.IsCurrent = true;
            }

            return View(viewModel);
        }

        [Route("QuickAuthentication/edit/{userId:int?}")]
        public ActionResult Edit(int? userId)
        {
            var viewModel = userId.HasValue ? DevelopmentService.Value.Get(userId.Value) : new DevelopmentUserViewModel();

            return View(viewModel);
        }

        [Route("QuickAuthentication/edit/{userId:int?}")]
        [HttpPost]
        public ActionResult Edit(int? userId, DevelopmentUserViewModel viewModel)
        {
            if (!userId.HasValue)
            {
                userId = SecurityManager.Value.LoginNewAnonymous();
            }

            DevelopmentService.Value.SetUserName(userId.Value, viewModel.Name);

            return RedirectToAction("Index");
        }

        [Route("QuickAuthentication/login/{userId:int:min(1)}")]
        public ActionResult Login(int userId)
        {
            if (DevelopmentService.Value.CanLogin(userId))
            {
                SecurityManager.Value.SetUserId(userId);
            }

            return RedirectToAction("Index");
        }
    }
}