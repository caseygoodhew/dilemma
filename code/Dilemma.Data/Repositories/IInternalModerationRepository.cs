using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Moderation repository methods that should not be externally available.
    /// </summary>
    internal interface IInternalModerationRepository : IModerationRepository
    {
        /// <summary>
        /// To be called when a question is created.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="question">The question that was created.</param>
        void OnQuestionCreated(DilemmaContext context, Question question);

        /// <summary>
        /// To be called when an answer is created.
        /// </summary>
        /// <param name="context">The context to run the queries against.</param>
        /// <param name="answer">The answer that was created.</param>
        void OnAnswerCreated(DilemmaContext context, Answer answer);
    }
}