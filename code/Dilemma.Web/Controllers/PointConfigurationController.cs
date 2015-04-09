using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    //[AllowUserRole(UserRole.Administrator)]
    public class PointConfigurationController : DilemmaBaseController
    {
        private static readonly Lazy<IAdministrationService> AdministrationService = Locator.Lazy<IAdministrationService>();

        //
        // GET: /PointConfiguration/
        public ActionResult Index()
        {
            return View(AdministrationService.Value.GetPointConfigurations());
        }

        //
        // GET: /PointConfiguration/Edit/5
        public ActionResult Edit(int id)
        {
            return View(AdministrationService.Value.GetPointConfiguration(id));
        }

        //
        // POST: /PointConfiguration/Edit/5
        [HttpPost]
        public ActionResult Edit(PointConfigurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AdministrationService.Value.SetPointConfiguration(viewModel);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
    }
}