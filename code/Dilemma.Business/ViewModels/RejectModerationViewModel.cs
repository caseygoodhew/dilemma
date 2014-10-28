namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// Reject moderation view model.
    /// </summary>
    public class RejectModerationViewModel
    {
        /// <summary>
        /// Gets or sets the moderation id.
        /// </summary>
        public int ModerationId { get; set; }

        /// <summary>
        /// Gets or sets the rejection message.
        /// </summary>
        public string Message { get; set; }
    }
}
