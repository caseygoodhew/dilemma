using System;
using System.Data.Entity.Spatial;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The user model.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="UserType"/>.
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// Gets or sets the number of total points that the user has.
        /// </summary>
        public int TotalPoints { get; set; }
        
        /// <summary>
        /// Gets or sets the number of historic points that the user has.
        /// </summary>
        public int HistoricPoints { get; set; }

        public int HistoricQuestions { get; set; }

        public int HistoricAnswers { get; set; }

        public int HistoricStarVotes { get; set; }

        public int HistoricPopularVotes { get; set; }
        
        
    }
}
