using Dilemma.Common;
using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalQuestionRepository : IQuestionRepository
    {
        void UpdateAnswerState(DilemmaContext context, int answerId, ModerationState moderationState);

        void UpdateQuestionState(DilemmaContext context, int questionId, ModerationState moderationState);
    }
}