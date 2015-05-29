using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal class InternalModerationRouter : IInternalModerationRouter
    {
        public void OnQuestionCreated(IMessenger<QuestionDataAction> messenger)
        {
            var messageContext = messenger.GetContext<QuestionMessageContext>(QuestionDataAction.Created);

            if (messageContext.Question.WebPurifyAttempted && messageContext.Question.PassedWebPurify)
            {
                return;
            }

            messenger.Forward();
        }

        public void OnAnswerCreated(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.Created);

            if (messageContext.Answer.WebPurifyAttempted && messageContext.Answer.PassedWebPurify)
            {
                return;
            }

            messenger.Forward();
        }

        public void OnFollowupCreated(IMessenger<FollowupDataAction> messenger)
        {
            var messageContext = messenger.GetContext<FollowupMessageContext>(FollowupDataAction.Created);

            if (messageContext.Followup.WebPurifyAttempted && messageContext.Followup.PassedWebPurify)
            {
                return;
            }

            messenger.Forward();
        }
    }
}