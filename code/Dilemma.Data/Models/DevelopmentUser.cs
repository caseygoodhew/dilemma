using System;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// A named user for development and testing purposes.
    /// </summary>
    public class DevelopmentUser
    {
        public int DevelopmentUserId { get; set; }

        public int UserId { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}