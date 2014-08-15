using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class SystemConfigurationMap : EntityTypeConfiguration<SystemConfiguration>
    {
        public SystemConfigurationMap()
        {
            HasKey(p => p.Id);
            Property(p => p.MaxAnswers).IsRequired();
            Property(p => p.QuestionLifetime).IsRequired();
        }
    }
}
