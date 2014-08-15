using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Dilemma.Data.EntityFramework.DilemmaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Dilemma.Data.EntityFramework.DilemmaContext context)
        {
            DilemmaInitializer.Seeder(context);
        }
    }
}
