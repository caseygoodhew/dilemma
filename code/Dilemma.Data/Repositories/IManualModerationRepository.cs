using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Publicly available moderation repository methods.
    /// </summary>
    public interface IManualModerationRepository
    {
        /// <summary>
        /// Gets the next item for moderation.
        /// </summary>
        /// <typeparam name="T">The type to convert the output to.</typeparam>
        /// <returns>The converted <see cref="Moderation"/></returns>
        T GetNext<T>() where T : class;

        /// <summary>
        /// Gets the next item to be moderated for the specified user.
        /// </summary>
        /// <typeparam name="T">The type to convert the output to.</typeparam>
        /// <param name="userId">The user id to get the next moderation for.</param>
        /// <returns>The converted <see cref="Moderation"/></returns>
        T GetNextForUser<T>(int userId) where T : class;

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

        T GetQuestionHistory<T>(int userId, int questionId) where T : class;

        T GetAnswerHistory<T>(int userId, int answerId) where T : class;

        T GetFollowupHistory<T>(int userId, int followupId) where T : class;

        void ReportQuestion(int userId, int questionId, ReportReason reportReason);

        void ReportAnswer(int userId, int answerId, ReportReason reportReason);

        void ReportFollowup(int userId, int followupId, ReportReason reportReason);
    }
}