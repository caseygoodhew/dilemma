using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            HasKey(p => p.QuestionId);
            
            Property(p => p.Text).IsRequired().HasMaxLength(2000);
            Property(p => p.MaxAnswers).IsRequired();
            Property(p => p.CreatedDateTime).IsRequired();
            Property(p => p.ClosesDateTime).IsRequired();
            
            HasRequired(x => x.Category).WithMany().WillCascadeOnDelete(false);
            HasMany(x => x.Answers).WithRequired(x => x.Question);
            
            Ignore(p => p.AnswerCount);
        }
    }
}
