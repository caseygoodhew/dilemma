using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class ReportedPostMap : EntityTypeConfiguration<ReportedPost>
    {
        public ReportedPostMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Reason).IsRequired();
            Property(p => p.ReportedDateTime).IsRequired();
            
            HasRequired(p => p.ByUser).WithMany().WillCascadeOnDelete(false);
            HasOptional(p => p.Question).WithMany().WillCascadeOnDelete(false);
            HasOptional(p => p.Answer).WithMany().WillCascadeOnDelete(false);
        }
    }
}