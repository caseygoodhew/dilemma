using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Data.Models.Virtual;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="UserStatisticsViewModel"/>.
    /// </summary>
    public static class UserStatisticsViewModelConverter
    {
        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();
        
        /// <summary>
        /// Converts a <see cref="UserStatistics"/> to a <see cref="UserStatisticsViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="UserStatistics"/> to convert.</param>
        /// <returns>The resultant <see cref="UserStatisticsViewModel"/>.</returns>
        public static UserStatisticsViewModel FromUserStatistics(UserStatistics model)
        {
            var ranks = SiteService.Value.GetRanks().OrderBy(x => x.Level);
            var rank = SiteService.Value.GetRankByPoints(model.TotalPoints);
            var nextRank = ranks.FirstOrDefault(x => x.Level == rank.Level + 1) ?? ranks.Last();
            var numberOfRanks = ranks.Count();

            var percentage = 100;

            if (rank.Level != nextRank.Level)
            {
                // base percentage of the number of ranks that I've already achieved plus the percentage of the way that I am to the next rank
                percentage = (100 * (rank.Level - 1) / (numberOfRanks - 1))
                                 + ((100 / (numberOfRanks - 1)) * (model.TotalPoints - rank.PointsRequired) / (nextRank.PointsRequired - rank.PointsRequired));
            }

            return new UserStatisticsViewModel
                       {
                           UserId = model.UserId,
                           TotalQuestions = model.TotalQuestions,
                           TotalAnswers = model.TotalAnswers,
                           TotalPoints = model.TotalPoints,
                           TotalStarVotes = model.TotalStarVotes,
                           TotalPopularVotes = model.TotalPopularVotes,
                           UserRank = new UserRankViewModel
                                          {
                                              Rank = rank,
                                              Percentage = percentage
                                          }
                       };
        }
    }
}