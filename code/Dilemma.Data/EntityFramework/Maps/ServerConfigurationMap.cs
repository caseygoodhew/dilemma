using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="SystemConfiguration"/> entity configuration.
    /// </summary>
    public class ServerConfigurationMap : EntityTypeConfiguration<ServerConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigurationMap" /> class.
        /// </summary>
        public ServerConfigurationMap()
        {
            HasKey(p => p.Id);
            Property(p => p.Name).IsRequired().HasMaxLength(100);
            Property(p => p.ServerRole).IsRequired();
        }
    }
}