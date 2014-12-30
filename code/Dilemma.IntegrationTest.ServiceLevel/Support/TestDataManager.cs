using System.IO;
using System.Web;

using Dilemma.Initialization;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;
using Disposable.Web.Caching;

namespace Dilemma.IntegrationTest.ServiceLevel.Support
{
    public class TestDataManager
    {
        private bool _initialized;
        
        internal TestDataManager() {}
        
        public void Prepare()
        {
            if (!_initialized)
            {
                DilemmaCore.Initialize();

                var locator = Locator.Current as Locator;
                
                var authenticationManager = new TestAuthenticationManager();
                locator.Register<IAuthenticationManager>(() => authenticationManager);

                var timeSource = new TimeWarpSource();
                locator.Register<ITimeSource>(() => timeSource);
                locator.Register<ITimeWarpSource>(() => timeSource);

                _initialized = true;
            }

            ResetAll();
        }

        private static void ResetAll()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://dilemma.integration.test", ""),
                new HttpResponse(new StringWriter())
            );
            
            ObjectDictionary.Reset();
            Locator.Get<IAuthenticationManager>().SignOut();
            Locator.Get<ITimeWarpSource>().DoThe(TimeWarpTo.TheHereAndNow);
            

            RequestCache.Current.ExpireAll();
        }
    }
}
