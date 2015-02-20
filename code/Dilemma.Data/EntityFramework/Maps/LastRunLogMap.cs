using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="LastRunLog"/> entity configuration.
    /// </summary>
    public class LastRunLogMap : EntityTypeConfiguration<LastRunLog>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LastRunLogMap" /> class.
        /// </summary>
        public LastRunLogMap()
        {
            HasKey(p => p.Id);
            Property(p => p.ExpireAnswerSlots).IsOptional();
            Property(p => p.CloseQuestions).IsOptional();
            Property(p => p.RetireOldQuestions).IsOptional();
        }
    }
}