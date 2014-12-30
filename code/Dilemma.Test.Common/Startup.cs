using Dilemma.Initialization;

namespace Dilemma.IntegrationTest.ServiceLevel
{
    public class Startup
    {
        private bool _initialized;
        
        internal Startup() {}
        
        public void Initialize()
        {
            ResetAll();
            
            if (_initialized)
            {
                return;
            }

            DilemmaCore.Initialize();

            _initialized = true;
        }

        private static void ResetAll()
        {
            Users.Reset();
        }
    }
}
