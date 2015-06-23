using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            LogErrors(exception);

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

        private static void LogErrors(Exception exception)
        {
            var headers =
                ToStringArray(
                    HttpContext.Current.Request.Headers.AllKeys.Select(
                        x =>
                            new KeyValuePair<string, string[]>(x,
                                HttpContext.Current.Request.Headers.GetValues(x) ?? new[] {string.Empty})));

            var requestBody = ToStringArray(new[]
            {
                new KeyValuePair<string, string>("AnonymousID", HttpContext.Current.Request.AnonymousID),
                new KeyValuePair<string, string>("AppRelativeCurrentExecutionFilePath",
                    HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath),
                new KeyValuePair<string, string>("ApplicationPath", HttpContext.Current.Request.ApplicationPath),
                new KeyValuePair<string, string>("Browser", HttpContext.Current.Request.Browser.ToString()),
                new KeyValuePair<string, string>("ContentEncoding",
                    HttpContext.Current.Request.ContentEncoding.ToString()),
                new KeyValuePair<string, string>("ContentLength", HttpContext.Current.Request.ContentLength.ToString()),
                new KeyValuePair<string, string>("ContentType", HttpContext.Current.Request.ContentType),
                new KeyValuePair<string, string>("HttpMethod", HttpContext.Current.Request.HttpMethod),
                new KeyValuePair<string, string>("IsAuthenticated",
                    HttpContext.Current.Request.IsAuthenticated.ToString()),
                new KeyValuePair<string, string>("IsLocal", HttpContext.Current.Request.IsLocal.ToString()),
                new KeyValuePair<string, string>("IsSecureConnection",
                    HttpContext.Current.Request.IsSecureConnection.ToString()),
                new KeyValuePair<string, string>("Path", HttpContext.Current.Request.Path),
                new KeyValuePair<string, string>("PathInfo", HttpContext.Current.Request.PathInfo),
                new KeyValuePair<string, string>("RawUrl", HttpContext.Current.Request.RawUrl),
                new KeyValuePair<string, string>("RequestType", HttpContext.Current.Request.RequestType),
                new KeyValuePair<string, string>("Url", HttpContext.Current.Request.Url.ToString()),
                new KeyValuePair<string, string>("UrlReferrer",
                    HttpContext.Current.Request.UrlReferrer == null
                        ? string.Empty
                        : HttpContext.Current.Request.UrlReferrer.
                            ToString()),
                new KeyValuePair<string, string>("UserAgent", HttpContext.Current.Request.UserAgent),
                new KeyValuePair<string, string>("UserHostAddress", HttpContext.Current.Request.UserHostAddress),
                new KeyValuePair<string, string>("UserHostName", HttpContext.Current.Request.UserHostName),
                new KeyValuePair<string, string>("UserLanguages",
                    string.Join(", ", HttpContext.Current.Request.UserLanguages ?? Enumerable.Empty<string>())),
            });

            var form =
                ToStringArray(
                    HttpContext.Current.Request.Form.AllKeys.Select(
                        x =>
                            new KeyValuePair<string, string[]>(x,
                                HttpContext.Current.Request.Form.GetValues(x) ?? new[] {string.Empty})));

            var userClaims = HttpContext.Current.Request.LogonUserIdentity == null
                ? Enumerable.Empty<string>()
                : ToStringArray(
                    HttpContext.Current.Request.LogonUserIdentity.UserClaims.Select(
                        x =>
                            new KeyValuePair<string, string>(x.Type, x.Value)));

            var requestParams =
                ToStringArray(
                    HttpContext.Current.Request.Params.AllKeys.Select(
                        x =>
                            new KeyValuePair<string, string[]>(x,
                                HttpContext.Current.Request.Params.GetValues(x) ?? new[] {string.Empty})));

            var serverVariables =
                ToStringArray(
                    HttpContext.Current.Request.ServerVariables.AllKeys.Select(
                        x =>
                            new KeyValuePair<string, string[]>(x,
                                HttpContext.Current.Request.ServerVariables.GetValues(x) ?? new[] {string.Empty})));


            var message = ToMessage(new[]
            {
                new KeyValuePair<string, IEnumerable<string>>("EXCEPTION", new[] { exception.ToString() }),
                new KeyValuePair<string, IEnumerable<string>>("REQUEST HEADERS", headers),
                new KeyValuePair<string, IEnumerable<string>>("REQUEST BODY", requestBody),
                new KeyValuePair<string, IEnumerable<string>>("REQUEST FORM", form),
                new KeyValuePair<string, IEnumerable<string>>("REQUEST PARAMS", requestParams),
                new KeyValuePair<string, IEnumerable<string>>("USER CLAIMS", userClaims),
                new KeyValuePair<string, IEnumerable<string>>("SERVER VARIABLES", serverVariables)
            });
            
            Locator.Get<ILogger>().ErrorException(message, exception);
        }

        private static IEnumerable<string> ToStringArray(IEnumerable<KeyValuePair<string, string>> values)
        {
            return ToStringArray(values.Select(x => new KeyValuePair<string, string[]>(x.Key, new[] { x.Value })));
        }

        private static IEnumerable<string> ToStringArray(IEnumerable<KeyValuePair<string, string[]>> values)
        {
            var valuesList = values.ToList();

            if (valuesList.Count == 0)
            {
                return Enumerable.Empty<string>();
            }

            var keyPadding = string.Format("{{0,-{0}}}", valuesList.Max(x => x.Key.Length));

            return valuesList
                .Select(x => new { x.Key, Value = x.Value.Any() ? x.Value : new[] { string.Empty } })
                .SelectMany(
                    x =>
                        x.Value.Select(
                            (v, i) =>
                                new { Key = i == 0 ? x.Key : string.Empty, Seperator = i == 0 ? "=" : " ", Value = v }))
                .Select(x => string.Format("{0} {1} {2}", string.Format(keyPadding, x.Key), x.Seperator, x.Value));
        }

        private static string ToMessage(IEnumerable<KeyValuePair<string, IEnumerable<string>>> values)
        {
            var builder = new StringBuilder();

            foreach (var value in values)
            {
                builder.AppendLine("***************************************");
                builder.AppendLine(string.Format("* {0}", value.Key));
                builder.AppendLine("***************************************");
                builder.AppendLine();
                value.Value.ToList().ForEach(x => builder.AppendLine(x));
                builder.AppendLine();
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
