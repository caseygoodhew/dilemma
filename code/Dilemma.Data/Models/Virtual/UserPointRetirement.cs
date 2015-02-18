namespace Dilemma.Data.Models.Virtual
{
    /// <summary>
    /// Helper table to retire user points. Managed by EF but used by stored procedures.
    /// </summary>
    public class UserPointRetirement
    {
        /// <summary>
        /// Get or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user id for the points being retired.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the total points for a user.
        /// </summary>
        public int TotalPoints { get; set; }
    }
}