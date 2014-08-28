using System.Data.Entity.Migrations;

using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Migrations
{
    /// <summary>
    /// Specifies configuration and seed options for migration operations.
    /// </summary>
    internal sealed class DilemmaDbMigrationConfiguration : DbMigrationsConfiguration<DilemmaContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DilemmaDbMigrationConfiguration" /> class.
        /// </summary>
        public DilemmaDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Seeds the database post migration.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/> to seed.</param>
        protected override void Seed(DilemmaContext context)
        {
            DilemmaInitializer.Seeder(context);
        }
    }
}
