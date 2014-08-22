using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class AnswerMap : EntityTypeConfiguration<Answer>
    {
        public AnswerMap()
        {
            HasKey(p => p.AnswerId);
            Property(p => p.Text).HasMaxLength(2000);
            Property(p => p.CreatedDateTime).IsRequired();
        }
    }
}
