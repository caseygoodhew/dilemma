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

    /// <summary>
    /// Helper table to retire questions. Managed by EF but used by stored procedures.
    /// </summary>
    public class VoteCountRetirement
    {
        /// <summary>
        /// Get or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        public int AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the vote count.
        /// </summary>
        public int Votes { get; set; }
    }
}
