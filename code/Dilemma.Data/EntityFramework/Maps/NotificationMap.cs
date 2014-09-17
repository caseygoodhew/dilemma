﻿using System.Data.Entity.ModelConfiguration;

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

            HasRequired(p => p.ForUser).WithMany().WillCascadeOnDelete(false);
            
            Property(p => p.NotificationType).IsRequired();
            Property(p => p.NotificationTypeKey).IsRequired();

            Property(p => p.Controller).IsRequired();
            Property(p => p.Action).IsRequired();

            Property(p => p.RouteDataKey).IsOptional();
            Property(p => p.RouteDataValue).IsOptional();

            Property(p => p.CreatedDateTime).IsRequired();
            Property(p => p.ActionedDateTime).IsOptional();
        }
    }
}
