using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models.Virtual;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class UserStatisticsMap : EntityTypeConfiguration<UserStatistics>
    {
        public UserStatisticsMap()
        {
            HasKey(x => x.UserId);
        }
    }
}