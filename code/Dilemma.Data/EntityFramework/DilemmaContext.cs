using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

using Dilemma.Data.EntityFramework.Maps;
using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework
{
    public class DilemmaContext : DbContext
    {
        public DilemmaContext() : base("DilemmaContext") {}
        
        public DbSet<Question> Questions { get; set; }

        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }

        public DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new SystemConfigurationMap());
        }

        public static void Startup()
        {
            Database.SetInitializer<DilemmaContext>(null);
        }
    }
}
