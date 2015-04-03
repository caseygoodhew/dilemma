using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class RankMap : EntityTypeConfiguration<Rank>
    {
        public RankMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Name).IsRequired();
            Property(p => p.PointsRequired).IsRequired();

            Ignore(p => p.Level);
        }
    }
}