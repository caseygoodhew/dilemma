using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Vote"/> entity configuration.
    /// </summary>
    public class VoteMap : EntityTypeConfiguration<Vote>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteMap" /> class.
        /// </summary>
        public VoteMap()
        {
            HasKey(p => p.Id);
            Property(p => p.CreatedDateTime).IsRequired();

            HasRequired(p => p.Question).WithMany().WillCascadeOnDelete(false);
            HasRequired(p => p.Answer).WithMany().WillCascadeOnDelete(false);
            HasRequired(p => p.User).WithMany().WillCascadeOnDelete(false);
        }
    }
}