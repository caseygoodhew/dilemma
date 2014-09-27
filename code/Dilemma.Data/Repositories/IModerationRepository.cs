using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    public interface IModerationRepository
    {
        void OnQuestionCreated(Question question);

        void OnAnswerCreated(Answer answer);
    }
}