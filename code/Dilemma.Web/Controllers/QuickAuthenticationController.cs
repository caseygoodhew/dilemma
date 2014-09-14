using System;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Security;
using Dilemma.Security.Development;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [DenyProductionAccess]
    public sealed class QuickAuthenticationController : DilemmaBaseController
    {
        private static readonly Lazy<IDevelopmentService> DevelopmentService = new Lazy<IDevelopmentService>(Locator.Current.Instance<IDevelopmentService>);

        private static readonly Lazy<ISecurityManager> SecurityManager = new Lazy<ISecurityManager>(Locator.Current.Instance<ISecurityManager>);
            
        [Route("quickauth")]
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

        [Route("quickauth/edit")]
        public ActionResult Edit()
        {
            return Edit(null as int?);
        }

        [Route("quickauth/edit")]
        [HttpPost]
        public ActionResult Edit(DevelopmentUserViewModel viewModel)
        {
            return Edit(null, viewModel);
        }

        [Route("quickauth/edit/{userId:int:min(1)}")]
        public ActionResult Edit(int? userId)
        {
            var viewModel = userId.HasValue ? DevelopmentService.Value.Get(userId.Value) : new DevelopmentUserViewModel();
            
            return View(viewModel);
        }

        [Route("quickauth/edit/{userId:int:min(1)}")]
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

        [Route("quickauth/login/{userId:int:min(1)}")]
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