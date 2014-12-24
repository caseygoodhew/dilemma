using Dilemma.Common;

using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalNotificationDistributor
    {
        /// <summary>
        /// To be called when an answer state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{AnswerDataAction}"/>.</param>
        void OnAnswerStateChange(IMessenger<AnswerDataAction> messenger);

        /// <summary>
        /// To be called when a moderation state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{ModerationState}"/>.</param>
        void OnModerationRejected(IMessenger<ModerationState> messenger);
    }
}