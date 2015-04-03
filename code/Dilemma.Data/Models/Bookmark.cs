using System;

namespace Dilemma.Data.Models
{
    public class Bookmark
    {
        public int Id { get; set; }
        
        public User User { get; set; }

        public Question Question { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
