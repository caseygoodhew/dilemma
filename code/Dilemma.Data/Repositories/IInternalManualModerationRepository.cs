using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Moderation repository methods that should not be externally available.
    /// </summary>
    internal interface IInternalManualModerationRepository : IManualModerationRepository
    {
        /// <summary>
        /// To be called when a question is created.
        /// </summary>
        /// <param name="messenger">The question <see cref="IMessenger{T}"/>.</param>
        void OnQuestionCreated(IMessenger<QuestionDataAction> messenger);

        /// <summary>
        /// To be called when an answer is created.
        /// </summary>
        /// <param name="messenger">The answer <see cref="IMessenger{T}"/>.</param>
        void OnAnswerCreated(IMessenger<AnswerDataAction> messenger);
    }
}