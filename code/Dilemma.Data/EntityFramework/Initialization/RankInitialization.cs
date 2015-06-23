using System.Linq;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Initialization
{
    /// <summary>
    /// Seeds the <see cref="Rank"/>s in the database.
    /// </summary>
    internal static class RankInitialization
    {
        /// <summary>
        /// Seeds the <see cref="Rank"/>s in the database.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/> to seed.</param>
        internal static void Seed(DilemmaContext context)
        {
            if (context.Ranks.Any())
            {
                return; 
            }

            context.Ranks.Add(
                new Rank
                    {
                        PointsRequired = 0,
                        Name = "Scholar"
                    });

            context.Ranks.Add(
                new Rank
                    {
                        PointsRequired = 1000,
                        Name = "Wise Owl"
                    });

            context.Ranks.Add(
                new Rank
                    {
                        PointsRequired = 5000,
                        Name = "Guru"
                    });

            context.Ranks.Add(
                new Rank
                    {
                        PointsRequired = 15000,
                        Name = "Guardian"
                    });

            context.Ranks.Add(
                new Rank
                    {
                        PointsRequired = 50000,
                        Name = "Deity"
                    });
        }
    }
}