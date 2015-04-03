using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;
using Dilemma.Data.Models.Virtual;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="QuestionRetirement"/> entity configuration.
    /// </summary>
    public class QuestionRetirementMap : EntityTypeConfiguration<QuestionRetirement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionRetirementMap" /> class.
        /// </summary>
        public QuestionRetirementMap()
        {
            HasKey(p => p.Id);

            Property(p => p.QuestionId).IsRequired();
        }
    }

    public class VoteCountRetirementMap : EntityTypeConfiguration<VoteCountRetirement>
    {
        public VoteCountRetirementMap()
        {
            HasKey(p => p.Id);

            Property(p => p.QuestionId).IsRequired();
            Property(p => p.AnswerId).IsRequired();
            Property(p => p.Votes).IsRequired();
        }
    }

    /// <summary>
    /// <see cref="UserPointRetirement"/> entity configuration.
    /// </summary>
    public class UserPointRetirementMap : EntityTypeConfiguration<UserPointRetirement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPointRetirementMap" /> class.
        /// </summary>
        public UserPointRetirementMap()
        {
            HasKey(p => p.Id);

            Property(p => p.UserId).IsRequired();

            Property(p => p.TotalPoints).IsRequired();
        }
    }

    /// <summary>
    /// <see cref="ModerationRetirementMap"/> entity configuration.
    /// </summary>
    public class ModerationRetirementMap : EntityTypeConfiguration<ModerationRetirement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModerationRetirementMap" /> class.
        /// </summary>
        public ModerationRetirementMap()
        {
            HasKey(p => p.Id);

            Property(p => p.ModerationId).IsRequired();
        }
    }
}