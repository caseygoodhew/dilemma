namespace Dilemma.Data.Models.Virtual
{
    /// <summary>
    /// Helper table to retire moderation entries. Managed by EF but used by stored procedures.
    /// </summary>
    public class ModerationRetirement
    {
        /// <summary>
        /// Get or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the moderation id being retired.
        /// </summary>
        public int ModerationId { get; set; }
    }
}