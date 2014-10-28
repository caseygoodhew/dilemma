using Dilemma.Common;
using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Question repository methods that should not be externally available.
    /// </summary>
    internal interface IInternalQuestionRepository : IQuestionRepository
    {
        /// <summary>
        /// Updates an <see cref="AnswerState"/> based on the provided <see cref="ModerationState"/>.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="answerId">The id of the answer to update.</param>
        /// <param name="moderationState">The <see cref="ModerationState"/>.</param>
        void UpdateAnswerState(DilemmaContext context, int answerId, ModerationState moderationState);

        /// <summary>
        /// Updates a <see cref="QuestionState"/> based on the provided <see cref="ModerationState"/>.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="questionId">The id of the question to update.</param>
        /// <param name="moderationState">The <see cref="ModerationState"/>.</param>
        void UpdateQuestionState(DilemmaContext context, int questionId, ModerationState moderationState);
    }
}