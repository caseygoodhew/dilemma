namespace Dilemma.Business.ViewModels
{
    public class UserStatisticsViewModel
    {
        public int UserId { get; set; }

        public int TotalQuestions { get; set; }

        public int TotalAnswers { get; set; }

        public int TotalPoints { get; set; }

        public int TotalStarVotes { get; set; }

        public int TotalPopularVotes { get; set; }

        public UserRankViewModel UserRank { get; set; }
    }
}