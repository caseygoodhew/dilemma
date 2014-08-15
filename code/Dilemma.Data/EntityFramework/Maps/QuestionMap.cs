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
            HasRequired(x => x.Category).WithMany().WillCascadeOnDelete(false);
        }
    }
}
