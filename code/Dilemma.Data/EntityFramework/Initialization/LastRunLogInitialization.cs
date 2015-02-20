using System.Linq;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Initialization
{
    /// <summary>
    /// Seeds the <see cref="LastRunLog"/>s in the database.
    /// </summary>
    internal static class LastRunLogInitialization
    {
        /// <summary>
        /// Seeds the <see cref="LastRunLog"/>s in the database.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/> to seed.</param>
        internal static void Seed(DilemmaContext context)
        {
            if (!context.LastRunLog.Any())
            {
                context.LastRunLog.Add(new LastRunLog());
            }
        }
    }
}