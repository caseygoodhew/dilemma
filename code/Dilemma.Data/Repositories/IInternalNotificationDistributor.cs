using Dilemma.Common;

using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalNotificationDistributor
    {
        /// <summary>
        /// To be called when a question state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{QuestionDataAction}"/>.</param>
        void OnQuestionStateChange(IMessenger<QuestionDataAction> messenger);
        
        /// <summary>
        /// To be called when an answer state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{AnswerDataAction}"/>.</param>
        void OnAnswerStateChange(IMessenger<AnswerDataAction> messenger);

        /// <summary>
        /// To be called when an followup state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{FollowupDataAction}"/>.</param>
        void OnFollowupStateChange(IMessenger<FollowupDataAction> messenger);

        void OnVotingOpen(IMessenger<QuestionDataAction> messenger);

        void OnVoteCast(IMessenger<AnswerDataAction> messenger);

        void OnBestAnswerAwarded(IMessenger<AnswerDataAction> messenger);

        /// <summary>
        /// To be called when a moderation state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{ModerationState}"/>.</param>
        void OnModerationRejected(IMessenger<ModerationState> messenger);
    }
}