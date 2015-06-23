using System;
using Disposable.Common.ServiceLocator;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal class InternalModerationRouter : IInternalModerationRouter
    {
        private static readonly Lazy<IMessagePipe<QuestionDataAction>> QuestionMessagePipe = Locator.Lazy<IMessagePipe<QuestionDataAction>>();

        private static readonly Lazy<IMessagePipe<AnswerDataAction>> AnswerMessagePipe = Locator.Lazy<IMessagePipe<AnswerDataAction>>();

        private static readonly Lazy<IMessagePipe<FollowupDataAction>> FollowupMessagePipe = Locator.Lazy<IMessagePipe<FollowupDataAction>>();
        
        public void OnQuestionCreated(IMessenger<QuestionDataAction> messenger)
        {
            var messageContext = messenger.GetContext<QuestionMessageContext>(QuestionDataAction.Created);

            if (messageContext.Question.WebPurifyAttempted && messageContext.Question.PassedWebPurify)
            {
                var newMessageContext = new QuestionMessageContext(QuestionDataAction.StateChanged, messageContext.DataContext, messageContext.Question);
                QuestionMessagePipe.Value.Announce(newMessageContext);
                return;
            }

            messenger.Forward();
        }

        public void OnAnswerCreated(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.Created);

            if (messageContext.Answer.WebPurifyAttempted && messageContext.Answer.PassedWebPurify)
            {
                var newMessageContext = new AnswerMessageContext(AnswerDataAction.StateChanged, messageContext.DataContext, messageContext.Answer);
                AnswerMessagePipe.Value.Announce(newMessageContext);
                return;
            }

            messenger.Forward();
        }

        public void OnFollowupCreated(IMessenger<FollowupDataAction> messenger)
        {
            var messageContext = messenger.GetContext<FollowupMessageContext>(FollowupDataAction.Created);

            if (messageContext.Followup.WebPurifyAttempted && messageContext.Followup.PassedWebPurify)
            {
                var newMessageContext = new FollowupMessageContext(FollowupDataAction.StateChanged, messageContext.DataContext, messageContext.Followup);
                FollowupMessagePipe.Value.Announce(newMessageContext);
                return;
            }

            messenger.Forward();
        }
    }
}