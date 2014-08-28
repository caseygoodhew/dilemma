using System;

using Dilemma.Data.EntityFramework.Initialization;

namespace Dilemma.Data.EntityFramework
{
    /// <summary>
    /// Todo: complete this xml comment if the class is actually used.
    /// </summary>
    public class DilemmaInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DilemmaContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DilemmaInitializer" /> class.
        /// </summary>
        public DilemmaInitializer()
        {
            throw new NotImplementedException("Never used?");
        }

        /// <summary>
        /// TODO: complete this comment if the class survives
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/></param>
        public static void Seeder(DilemmaContext context)
        {
            DatabaseInitialization.Initialize(context);
        }

        /// <summary>
        /// TODO: complete this comment if the class survives.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/></param>
        protected override void Seed(DilemmaContext context)
        {
            DatabaseInitialization.Initialize(context);
        }
    }
}
