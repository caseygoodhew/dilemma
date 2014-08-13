using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework
{
    public class DilemmaContext : DbContext
    {
        public DilemmaContext() : base("DilemmaContext") {}
        
        public DbSet<Question> Questions { get; set; }

        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
