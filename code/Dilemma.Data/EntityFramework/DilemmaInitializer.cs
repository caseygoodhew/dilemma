using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Dilemma.Common;
using Dilemma.Data.EntityFramework.Initialization;
using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework
{
    public class DilemmaInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DilemmaContext>
    {
        public static void Seeder(DilemmaContext context)
        {
            new DilemmaInitializer().Seed(context);
        }
        
        protected override void Seed(DilemmaContext context)
        {
            SystemConfigurationInitialization.Seed(context);
            CategoryInitialization.Seed(context);
        }
    }
}
