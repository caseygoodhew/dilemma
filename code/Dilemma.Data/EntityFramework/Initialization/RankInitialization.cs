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
            if (context.Rank.Any())
            {
                return; 
            }

            context.Rank.Add(
                new Rank
                    {
                        PointsRequired = 0,
                        Name = "Teacher"
                    });

            context.Rank.Add(
                new Rank
                    {
                        PointsRequired = 100,
                        Name = "Wise Owl"
                    });

            context.Rank.Add(
                new Rank
                    {
                        PointsRequired = 1000,
                        Name = "Guru"
                    });

            context.Rank.Add(
                new Rank
                    {
                        PointsRequired = 5000,
                        Name = "Angel"
                    });

            context.Rank.Add(
                new Rank
                    {
                        PointsRequired = 20000,
                        Name = "Deity"
                    });
        }
    }
}