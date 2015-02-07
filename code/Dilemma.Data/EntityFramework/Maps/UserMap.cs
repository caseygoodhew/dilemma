using System.Data.Entity.ModelConfiguration;
using System.Security.Cryptography.X509Certificates;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="User"/> entity configuration.
    /// </summary>
    public class UserMap : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMap" /> class.
        /// </summary>
        public UserMap()
        {
            HasKey(p => p.UserId);

            Property(p => p.CreatedDateTime).IsRequired();
            Property(p => p.UserType).IsRequired();
            Property(p => p.HistoricPoints).IsRequired();

            Ignore(p => p.TotalPoints);
        }
    }
}
