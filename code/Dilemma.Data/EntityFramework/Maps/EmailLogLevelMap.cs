using System.Data.Entity.ModelConfiguration;
using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class EmailLogLevelMap : EntityTypeConfiguration<EmailLogLevel>
    {
        public EmailLogLevelMap()
        {
            HasKey(p => p.Id);

            Property(p => p.LogLevel).IsRequired().HasMaxLength(20);
            Property(p => p.EnableEmails).IsRequired();
        }
    }
}