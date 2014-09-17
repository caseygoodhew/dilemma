﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Dilemma.Initialization;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DilemmaCore.Initialize();
            Registration.Register(Locator.Current as Locator);
        }
    }
}
