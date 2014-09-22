using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Notification"/> entity configuration.
    /// </summary>
    public class NotificationMap : EntityTypeConfiguration<Notification>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationMap" /> class.
        /// </summary>
        public NotificationMap()
        {
            HasKey(p => p.NotificationId);

            Property(p => p.NotificationType).IsRequired();
            
            HasRequired(p => p.ForUser).WithMany().WillCascadeOnDelete(false);
            HasOptional(p => p.Answer).WithMany().WillCascadeOnDelete(false);
            
            Property(p => p.CreatedDateTime).IsRequired();
            Property(p => p.ActionedDateTime).IsOptional();
        }
    }
}
