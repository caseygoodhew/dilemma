using System.ComponentModel.DataAnnotations;

namespace Dilemma.Data.Models.Virtual
{
    /// <summary>
    /// Helper class to represent a stored procedure. 
    /// </summary>
    public class UserStatistics
    {
        [Key]
        public int UserId { get; set; }

        public int TotalQuestions { get; set; }

        public int TotalAnswers { get; set; }

        public int TotalPoints { get; set; }

        public int TotalStarVotes { get; set; }

        public int TotalPopularVotes { get; set; }
    }
}