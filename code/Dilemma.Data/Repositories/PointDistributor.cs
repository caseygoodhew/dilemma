using System;

using Dilemma.Common;

using Disposable.Common.ServiceLocator;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal class PointDistributor : IInternalPointDistributor
    {
        private static readonly Lazy<IInternalPointsRepository> PointsRepository = Locator.Lazy<IInternalPointsRepository>();
        
        public void OnModerationApproved(IMessenger<ModerationState> messenger)
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
                    referenceId = messageContext.Moderation.Question.QuestionId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PointsRepository.Value.AwardPoints(messageContext.DataContext, messageContext.Moderation.ForUser.UserId, pointType, referenceId);

            messenger.Forward();
        }
    }
}