﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Security;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [UnlockableAccessFilter("AdministrationUnlockKey", "Home", "Index")]
    public class SystemServerConfigurationController : DilemmaBaseController
    {
        private static readonly Lazy<IAdministrationService> AdministrationService = Locator.Lazy<IAdministrationService>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        //
        // GET: /SystemConfiguration/
        public ActionResult Index()
        {
            return View(AdministrationService.Value.GetSystemServerConfiguration());
        }

        //
        // GET: /SystemConfiguration/
        public ActionResult RefreshCache()
        {
            AdministrationService.Value.ExpireCachedSystemServerConfiguration();
            return RedirectToAction("index");
        }

        public ActionResult TestErrorHandling()
        {
            throw new TestErrorHandlingException();
        }

        //
        // POST: /SystemConfiguration/
        [HttpPost]
        public ActionResult Index(SystemServerConfigurationViewModel systemServerConfiguration)
        {
            if (ModelState.IsValid)
            {
                AdministrationService.Value.SetSystemServerConfiguration(systemServerConfiguration);
                return RedirectToAction("Index");
            }

            return View(systemServerConfiguration);
        }
    }

    internal class TestErrorHandlingException : Exception { }
}