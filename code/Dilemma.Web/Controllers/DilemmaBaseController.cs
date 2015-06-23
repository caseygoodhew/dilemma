using System;
using System.Globalization;
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
                           UserStatistics = UserService.Value.GetUserStatistics(),
                           NewNotificationsCount = NotificationService.Value.CountNewNotifications(),
                           Notifications = NotificationService.Value.GetTopUnread(2)
                       };
        }

        protected static string NumberToText(decimal number)
        {
            if (number == 0)
            {
                return "zero";
            }

            if (number == 1)
            {
                return "one";
            }

            if (number == 2)
            {
                return "two";
            }

            if (number == 3)
            {
                return "three";
            }

            if (number == 4)
            {
                return "four";
            }

            if (number == 5)
            {
                return "five";
            }

            if (number == 6)
            {
                return "six";
            }

            if (number == 7)
            {
                return "seven";
            }

            if (number == 8)
            {
                return "eight";
            }

            if (number == 9)
            {
                return "nine";
            }

            return number.ToString(CultureInfo.CurrentCulture);
        }
    }
}