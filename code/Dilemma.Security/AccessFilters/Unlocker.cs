using System;
using System.Collections.Specialized;
using System.Web;

namespace Dilemma.Security.AccessFilters
{
    internal static class Unlocker
    {
        public static bool HasKey(HttpRequestBase request, string unlockKey)
        {
            if (HasKey(request.Params, unlockKey))
            {
                return true;
            }

            var referer = request.UrlReferrer;
            if (referer == null)
            {
                return false;
            }

            return HasKey(HttpUtility.ParseQueryString(referer.Query), unlockKey);
        }

        private static bool HasKey(NameValueCollection dict, string unlockKey)
        {
            return StringComparer.InvariantCultureIgnoreCase.Compare(unlockKey, dict["key"]) == 0;
        }
    }
}