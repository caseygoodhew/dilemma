using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Followup"/> entity configuration.
    /// </summary>
    public class FollowupMap : EntityTypeConfiguration<Followup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowupMap" /> class.
        /// </summary>
        public FollowupMap()
        {
            HasKey(p => p.FollowupId);
            Property(p => p.Text).HasMaxLength(2000);
            Property(p => p.CreatedDateTime).IsRequired();
            Property(p => p.FollowupState).IsRequired();

            HasRequired(p => p.Question).WithMany().WillCascadeOnDelete(false);

            HasRequired(p => p.User).WithMany().WillCascadeOnDelete(false);
        }
    }
}