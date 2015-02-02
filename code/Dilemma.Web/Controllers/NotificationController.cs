using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class NotificationController : Controller
    {
        private static readonly Lazy<INotificationService> NotificationService = Locator.Lazy<INotificationService>();
        
        //
        // GET: /Notification/
        public ActionResult Index()
        {
            return View(NotificationService.Value.GetAll());
        }
    }
}