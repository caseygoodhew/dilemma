using System;
using System.Web.SessionState;

using Disposable.Common;

namespace Dilemma.Security
{
    public static class SessionExtensions
    {
        public static T Get<T>(this HttpSessionState session, string key, Func<T> providerFunc)
        {
            Guard.ArgumentNotNull(session, "session");
            Guard.ArgumentNotNullOrEmpty(key, "key");

            T item;
            var result = session[key];

            if (result == null)
            {
                if (providerFunc == null)
                {
                    return default(T);
                }
                
                item = providerFunc();
                session[key] = item;
            }
            else
            {
                item = (T)result;
            }

            return item;
        }
    }
}
