using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Dilemma.Initialization;
using Dilemma.Logging;
using Dilemma.Web.Controllers;
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

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            var exception = Server.GetLastError();

            // Handle HTTP errors
            if (exception is HttpException)
            {
                // The Complete Error Handling Example generates
                // some errors using URLs with "NoCatch" in them;
                // ignore these here to simulate what would happen
                // if a global.asax handler were not implemented.
                if (exception.Message.Contains("NoCatch") || exception.Message.Contains("maxUrlLength"))
                {
                    return;
                }
            }

            // Log the exception and notify system operators
            Locator.Get<ILogger>().ErrorException(exception.ToString(), exception);

            // Clear the error from the server
            Server.ClearError();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action","Index");
            routeData.Values.Add("Summary","Error");
            //routeData.Values.Add("Description", ex.Message);
            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
