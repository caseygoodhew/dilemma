using Dilemma.Business.ViewModels;
using Dilemma.Data.Models.Virtual;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="UserStatisticsViewModel"/>.
    /// </summary>
    public static class UserStatisticsViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="UserStatistics"/> to a <see cref="UserStatisticsViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="UserStatistics"/> to convert.</param>
        /// <returns>The resultant <see cref="UserStatisticsViewModel"/>.</returns>
        public static UserStatisticsViewModel FromUserStatistics(UserStatistics model)
        {
            return new UserStatisticsViewModel
                       {
                           UserId = model.UserId,
                           TotalQuestions = model.TotalQuestions,
                           TotalAnswers = model.TotalAnswers,
                           TotalPoints = model.TotalPoints,
                           TotalStarVotes = model.TotalStarVotes,
                           TotalPopularVotes = model.TotalPopularVotes,
                           UserRank = new UserRankViewModel()
                       };
        }
    }
}