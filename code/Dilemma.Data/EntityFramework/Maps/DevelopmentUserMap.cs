using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="DevelopmentUser"/> entity configuration.
    /// </summary>
    public class DevelopmentUserMap : EntityTypeConfiguration<DevelopmentUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DevelopmentUserMap" /> class.
        /// </summary>
        public DevelopmentUserMap()
        {
            HasKey(p => p.DevelopmentUserId);
        }
    }
}
