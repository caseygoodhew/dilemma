using System.Web.Mvc;

using Dilemma.Security;

namespace Dilemma.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DilemmaAuthenticationAttribute());
        }
    }
}
