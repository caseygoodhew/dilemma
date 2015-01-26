using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

using Dilemma.Data.EntityFramework.Maps;
using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework
{
    /// <summary>
    /// The standard common context to use for connections to the Dilemma database.
    /// </summary>
    public class DilemmaContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DilemmaContext" /> class.
        /// </summary>
        public DilemmaContext() : base("DilemmaContext")
        {
        }
        
        /// <summary>
        /// Gets or sets the <see cref="Question"/> database set.
        /// </summary>
        public DbSet<Question> Questions { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="Answer"/> database set;
        /// </summary>
        public DbSet<Answer> Answers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SystemConfiguration"/> database set; 
        /// </summary>
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ServerConfiguration"/> database set; 
        /// </summary>
        public DbSet<ServerConfiguration> ServerConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Categories"/> database set;
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Users"/> database set;
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DevelopmentUsers"/> database set;
        /// </summary>
        public DbSet<DevelopmentUser> DevelopmentUsers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Notifications"/> database set;
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Moderation"/> database set;
        /// </summary>
        public DbSet<Moderation> Moderations { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ModerationEntry"/> database set;
        /// </summary>
        public DbSet<ModerationEntry> ModerationEntries { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PointConfigurations"/> database set;
        /// </summary>
        public DbSet<PointConfiguration> PointConfigurations { get; set; }

        /// <summary>
        /// Called the first that the the DbContext is used in each session.
        /// </summary>
        public static void Startup()
        {
            // From: http://msdn.microsoft.com/en-us/library/gg679461(v=vs.113).aspx
            //      Sets the database initializer to use for the given context type. 
            //      The database initializer is called when a the given DbContext type is used to access 
            //      a database for the first time. The default strategy for Code First contexts is an 
            //      instance of CreateDatabaseIfNotExists<TContext>.
            // This appears to be broken - if we allow EF6 to initialize the database it seems to 
            // always / frequently think that the bd is out of date. It throws an exception that a migrate 
            // needs to be performed, only to not find any differences during the migration generation phase.
            Database.SetInitializer<DilemmaContext>(null);
        }
        
        /// <summary>
        /// Configures the <see cref="DbModelBuilder"/> to use our configuration options and entity maps.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="DbModelBuilder"/> to configure.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new AnswerMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new SystemConfigurationMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new DevelopmentUserMap());
            modelBuilder.Configurations.Add(new NotificationMap());
            modelBuilder.Configurations.Add(new ModerationMap());
            modelBuilder.Configurations.Add(new ModerationEntryMap());
        }
    }
}
