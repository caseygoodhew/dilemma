using System;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// A named user for development and testing purposes.
    /// </summary>
    public class DevelopmentUser
    {
        /// <summary>
        /// Gets or sets the development user id.
        /// </summary>
        public int DevelopmentUserId { get; set; }

        /// <summary>
        /// Gets or sets the corresponding user id.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the development user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }
    }
}