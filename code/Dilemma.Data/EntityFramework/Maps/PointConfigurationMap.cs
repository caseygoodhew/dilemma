using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="PointConfiguration"/> entity configuration.
    /// </summary>
    public class PointConfigurationMap : EntityTypeConfiguration<PointConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigurationMap" /> class.
        /// </summary>
        public PointConfigurationMap()
        {
            HasKey(p => p.Id);
            Property(p => p.PointType);
            Property(p => p.Name).IsRequired();
            Property(p => p.Description).IsRequired();
            Property(p => p.Points).IsRequired();
        }
    }
}
