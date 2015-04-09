using System;
using System.Linq;

using Dilemma.Common;

using Disposable.Common.ServiceLocator;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal class PointDistributor : IInternalPointDistributor
    {
        private static readonly Lazy<IInternalPointsRepository> PointsRepository = Locator.Lazy<IInternalPointsRepository>();

        public void OnQuestionStateChange(IMessenger<QuestionDataAction> messenger)
        {
            messenger.Forward();
        }

        public void OnAnswerStateChange(IMessenger<AnswerDataAction> messenger)
        {
            messenger.Forward();
        }

        public void OnFollowupStateChange(IMessenger<FollowupDataAction> messenger)
        {
            messenger.Forward();
        }

        /*public void OnModerationApproved(IMessenger<ModerationState> messenger)
        {
            var messageContext = messenger.GetContext<ModerationMessageContext>(ModerationState.Approved);

            PointType pointType;
            int? referenceId;

            switch (messageContext.Moderation.ModerationFor)
            {
                case ModerationFor.Question:
                    pointType = PointType.QuestionAsked;
                    referenceId = messageContext.Moderation.Question.QuestionId;
                    break;
                case ModerationFor.Answer:
                    pointType = PointType.AnswerProvided;
                    var answerId = messageContext.Moderation.Answer.AnswerId;
                    referenceId =
                        messageContext.DataContext.Answers
                            .Where(x => x.AnswerId == answerId)
                            .Select(x => x.Question.QuestionId)
                            .Single();
                    break;
                case ModerationFor.Followup:
                    return;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PointsRepository.Value.AwardPoints(messageContext.DataContext, messageContext.Moderation.ForUser.UserId, pointType, referenceId);

            messenger.Forward();
        }

        public void OnVoteRegistered(IMessenger<VotingDataAction> messenger)
        {
            var messageContext = messenger.GetContext<VotingMessageContext>(VotingDataAction.VoteRegistered);

            PointsRepository.Value.AwardPoints(messageContext.DataContext, messageContext.UserId, PointType.VoteRegistered, messageContext.QuestionId);
            
            messenger.Forward();
        }

        public void OnVoteDeregistered(IMessenger<VotingDataAction> messenger)
        {
            var messageContext = messenger.GetContext<VotingMessageContext>(VotingDataAction.VoteDeregistered);

            PointsRepository.Value.RemovePoints(messageContext.DataContext, messageContext.UserId, PointType.VoteRegistered, messageContext.QuestionId);
            
            messenger.Forward();
        }

        public void OnStarVoteRegistered(IMessenger<VotingDataAction> messenger)
        {
            var messageContext = messenger.GetContext<VotingMessageContext>(VotingDataAction.StarVoteRegistered);

            PointsRepository.Value.AwardPoints(messageContext.DataContext, messageContext.AnswerUserId, PointType.StarVoteReceived, messageContext.QuestionId);

            messenger.Forward();
        }*/

        public void OnVotingOpen(IMessenger<QuestionDataAction> messenger)
        {
            messenger.Forward();
        }

        public void OnVoteCast(IMessenger<AnswerDataAction> messenger)
        {
            messenger.Forward();
        }

        public void OnBestAnswerAwarded(IMessenger<AnswerDataAction> messenger)
        {
            messenger.Forward();
        }
    }
}