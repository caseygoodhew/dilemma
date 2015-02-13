using System;

namespace Dilemma.Data.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public Question Question { get; set; }

        public Answer Answer { get; set; }
        
        public User User { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
