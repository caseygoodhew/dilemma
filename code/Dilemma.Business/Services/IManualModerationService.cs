using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Moderation service interface.
    /// </summary>
    public interface IManualModerationService
    {
        /// <summary>
        /// Gets the next item to be moderated.
        /// </summary>
        /// <returns>The <see cref="ModerationViewModel"/> or null.</returns>
        ModerationViewModel GetNext();

        /// <summary>
        /// Gets the next item to be moderated for the specified user.
        /// </summary>
        /// <param name="userId">The user id to get the next moderation for.</param>
        /// <returns>The <see cref="ModerationViewModel"/> or null.</returns>
        ModerationViewModel GetNextForUser(int userId);
        
        /// <summary>
        /// Marks a moderation item as being approved.
        /// </summary>
        /// <param name="moderationId">The moderation id to approve.</param>
        void Approve(int moderationId);

        /// <summary>
        /// Marks a moderation item as being rejected.
        /// </summary>
        /// <param name="moderationId">The moderation id to reject.</param>
        /// <param name="message">A message detailing the reason for rejection.</param>
        void Reject(int moderationId, string message);
    }
}
