using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="User"/> entity configuration.
    /// </summary>
    public class UserMap : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionMap" /> class.
        /// </summary>
        public UserMap()
        {
            HasKey(p => p.UserId);

            Property(p => p.CreatedDateTime).IsRequired();
            Property(p => p.UserType).IsRequired();
        }
    }
}
