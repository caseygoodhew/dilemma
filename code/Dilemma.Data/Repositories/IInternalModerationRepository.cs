using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalModerationRepository : IModerationRepository
    {
        void OnQuestionCreated(DilemmaContext context, Question question);

        void OnAnswerCreated(DilemmaContext context, Answer answer);
    }
}