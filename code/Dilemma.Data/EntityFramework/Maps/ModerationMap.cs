using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Moderation"/> entity configuration.
    /// </summary>
    public class ModerationMap : EntityTypeConfiguration<Moderation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModerationMap" /> class.
        /// </summary>
        public ModerationMap()
        {
            HasKey(p => p.ModerationId);

            Property(p => p.ModerationFor).IsRequired();
            
            HasRequired(p => p.ForUser).WithMany().WillCascadeOnDelete(false);
            // needs to remain optional for valid insert operation
            HasOptional(p => p.Question).WithMany().WillCascadeOnDelete(false);
            HasOptional(p => p.Answer).WithMany().WillCascadeOnDelete(false);

            HasMany(x => x.ModerationEntries).WithRequired(x => x.Moderation);
        }
    }
}