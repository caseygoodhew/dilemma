using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [QuestionSeederAccessFilter]
    public abstract class DilemmaBaseController : Controller
    {
        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();
        
        private static readonly Lazy<IUserService> UserService = Locator.Lazy<IUserService>();

        public static readonly Lazy<INotificationService> NotificationService = Locator.Lazy<INotificationService>();

        protected DilemmaBaseController()
        {
            ViewBag.Categories = SiteService.Value.GetCategories();
        }

        protected static SidebarViewModel GetSidebarViewModel()
        {
            return new SidebarViewModel
                       {
                           UserStatsViewModel = UserService.Value.GetUserStatistics(),
                           NewNotificationsCount = NotificationService.Value.CountNewNotifications(),
                           Notifications = NotificationService.Value.GetTopUnread(2)
                       };
        }
    }
}