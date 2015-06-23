using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data
{
    public static class Startup
    {
        private static bool _isInitialized;
        
        public static void Initialize()
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("Already initialized");
            }

            InitializeData();

            _isInitialized = true;
        }

        private static void InitializeData()
        {
            using (var context = new DilemmaContext())
            {
                DilemmaInitializer.Seeder(context);
                context.SaveChangesVerbose();
            }
        }
    }
}
