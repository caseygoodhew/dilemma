using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Answer"/> entity configuration.
    /// </summary>
    public class AnswerMap : EntityTypeConfiguration<Answer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerMap" /> class.
        /// </summary>
        public AnswerMap()
        {
            HasKey(p => p.AnswerId);
            Property(p => p.Text).HasMaxLength(2000);
            Property(p => p.CreatedDateTime).IsRequired();

            HasRequired(p => p.User).WithMany().WillCascadeOnDelete(false);

            Ignore(p => p.UserVotes);
        }
    }
}
