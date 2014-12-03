using System;

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
        /// Gets or sets the number of points that the user has.
        /// </summary>
        public int Points { get; set; }
    }
}
