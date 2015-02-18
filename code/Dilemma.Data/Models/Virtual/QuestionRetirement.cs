namespace Dilemma.Data.Models.Virtual
{
    /// <summary>
    /// Helper table to retire questions. Managed by EF but used by stored procedures.
    /// </summary>
    public class QuestionRetirement
    {
        /// <summary>
        /// Get or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the question being retired.
        /// </summary>
        public int QuestionId { get; set; }
    }
}
