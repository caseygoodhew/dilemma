using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class UserPointMap : EntityTypeConfiguration<UserPoint>
    {
        public UserPointMap()
        {
            HasKey(p => p.Id);

            Property(p => p.PointType).IsRequired();
            Property(p => p.PointsAwarded).IsRequired();
            Property(p => p.CreatedDateTime).IsRequired();

            HasRequired(p => p.ForUser).WithMany().WillCascadeOnDelete(false);
            HasOptional(p => p.RelatedQuestion).WithMany().WillCascadeOnDelete(false);
        }
    }
}