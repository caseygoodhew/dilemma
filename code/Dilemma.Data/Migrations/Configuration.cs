using System.Data.Entity.Migrations;

using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DilemmaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DilemmaContext context)
        {
            DilemmaInitializer.Seeder(context);
        }
    }
}
