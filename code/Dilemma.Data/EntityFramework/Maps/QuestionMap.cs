using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Question"/> entity configuration.
    /// </summary>
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionMap" /> class.
        /// </summary>
        public QuestionMap()
        {
            HasKey(p => p.QuestionId);
            
            Property(p => p.Text).IsRequired().HasMaxLength(2000);
            Property(p => p.MaxAnswers).IsRequired();
            Property(p => p.CreatedDateTime).IsRequired();
            Property(p => p.ClosesDateTime).IsRequired();
            Property(p => p.ClosedDateTime).IsOptional();
            
            HasRequired(x => x.Category).WithMany().WillCascadeOnDelete(false);
            HasMany(x => x.Answers).WithRequired(x => x.Question).WillCascadeOnDelete(false);
            
            HasRequired(p => p.User).WithMany().WillCascadeOnDelete(false);

            Ignore(p => p.TotalAnswers);
        }
    }
}
