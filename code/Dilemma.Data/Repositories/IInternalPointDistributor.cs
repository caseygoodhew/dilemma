using Dilemma.Common;

using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalPointDistributor
    {
        /// <summary>
        /// To be called when moderation is approved.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{ModerationState}"/>.</param>
        void OnModerationApproved(IMessenger<ModerationState> messenger);

        void OnVoteRegistered(IMessenger<VotingDataAction> messenger);

        void OnVoteDeregistered(IMessenger<VotingDataAction> messenger);

        void OnStarVoteRegistered(IMessenger<VotingDataAction> messenger);
    }
}