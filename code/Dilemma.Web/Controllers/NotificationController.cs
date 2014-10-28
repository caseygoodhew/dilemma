﻿using System;
using System.Web.Mvc;

using Dilemma.Business.Services;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
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