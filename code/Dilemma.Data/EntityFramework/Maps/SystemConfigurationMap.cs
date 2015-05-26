using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="SystemConfiguration"/> entity configuration.
    /// </summary>
    public class SystemConfigurationMap : EntityTypeConfiguration<SystemConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigurationMap" /> class.
        /// </summary>
        public SystemConfigurationMap()
        {
            HasKey(p => p.Id);
            Property(p => p.MaxAnswers).IsRequired();
            Property(p => p.QuestionLifetime).IsRequired();
            Property(p => p.SystemEnvironment).IsRequired();
            Property(p => p.RetireQuestionAfterDays).IsRequired();
            Property(p => p.ExpireAnswerSlotsAfterMinutes).IsRequired();
            Property(p => p.EnableWebPurify).IsRequired();
            Property(p => p.EmailErrors).IsRequired();
            Property(p => p.EmailErrorsTo).IsRequired();
        }
    }
}
