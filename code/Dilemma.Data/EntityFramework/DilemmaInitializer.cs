using System.Linq;

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
            if (!context.SystemConfiguration.Any())
            {
                context.SystemConfiguration.Add(new SystemConfiguration
                                                    {
                                                        MaxAnswers = 10
                                                    });
            }
        }
    }
}
