using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="ModerationEntry"/> entity configuration.
    /// </summary>
    public class ModerationEntryMap : EntityTypeConfiguration<ModerationEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModerationEntryMap" /> class.
        /// </summary>
        public ModerationEntryMap()
        {
            HasKey(p => p.ModerationEntryId);

            Property(p => p.EntryType).IsRequired();
            Property(p => p.Message).IsOptional();
            Property(p => p.CreatedDateTime).IsRequired();
            HasRequired(p => p.AddedByUser).WithMany().WillCascadeOnDelete(false);
        }
    }
}