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
            var messageContext = messenger.GetContext<QuestionMessageContext>(QuestionDataAction.StateChanged);

            if (messageContext.Question.QuestionState == QuestionState.Approved)
            {
                var pointType = PointType.QuestionAsked;

                var points = PointsRepository.Value.AwardPoints(
                    messageContext.DataContext,
                    messageContext.Question.User.UserId,
                    pointType,
                    messageContext.Question.QuestionId);

                messageContext.Dictionary[pointType.ToString()] = points;
            }

            messenger.Forward();
        }

        public void OnAnswerStateChange(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.StateChanged);

            if (messageContext.Answer.AnswerState == AnswerState.Approved)
            {
                var pointType = PointType.QuestionAnswered;
                
                var points = PointsRepository.Value.AwardPoints(
                    messageContext.DataContext,
                    messageContext.Answer.User.UserId,
                    pointType,
                    messageContext.Answer.Question.QuestionId);

                messageContext.Dictionary[pointType.ToString()] = points;
            }

            messenger.Forward();
        }
        
        public void OnBestAnswerAwarded(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.BestAnswerAwarded);

            var pointType = PointType.StarVoteReceived;

            var points = PointsRepository.Value.AwardPoints(
                messageContext.DataContext,
                messageContext.Answer.User.UserId,
                pointType,
                messageContext.Answer.Question.QuestionId);

            messageContext.Dictionary[pointType.ToString()] = points;
            
            messenger.Forward();
        }
    }
}