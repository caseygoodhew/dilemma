using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Publicly available moderation repository methods.
    /// </summary>
    public interface IModerationRepository
    {
        /// <summary>
        /// Gets the next item for moderation.
        /// </summary>
        /// <typeparam name="T">The type to convert the output to.</typeparam>
        /// <returns>The converted <see cref="Moderation"/></returns>
        T GetNext<T>() where T : class;

        /// <summary>
        /// Approves a moderation.
        /// </summary>
        /// <param name="userId">The id of the user approving the moderation.</param>
        /// <param name="moderationId">The id of the moderation to approve.</param>
        void Approve(int userId, int moderationId);

        /// <summary>
        /// Rejects a moderation.
        /// </summary>
        /// <param name="userId">The id of the user rejecting the moderation.</param>
        /// <param name="moderationId">The id of the moderation to reject.</param>
        /// <param name="message">A message as to why the moderation item is being rejected.</param>
        void Reject(int userId, int moderationId, string message);
    }
}