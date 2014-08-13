using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DilemmaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DilemmaContext context)
        {
            DilemmaInitializer.Seeder(context);
        }
    }
}
