using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalModerationRepository
    {
        void OnQuestionCreated(Question question);

        void OnAnswerCreated(Answer answer);
    }
}