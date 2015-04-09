using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalPointDistributor
    {
        void OnQuestionStateChange(IMessenger<QuestionDataAction> messenger);

        void OnAnswerStateChange(IMessenger<AnswerDataAction> messenger);

        void OnBestAnswerAwarded(IMessenger<AnswerDataAction> messenger);
    }
}