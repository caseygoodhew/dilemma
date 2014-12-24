using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data
{
    internal class AnswerMessageContext : DataMessageContext<AnswerDataAction>
    {
        public readonly Answer Answer;

        public AnswerMessageContext(AnswerDataAction messageType, DilemmaContext dataContext, Answer answer)
            : base(messageType, dataContext)
        {
            Answer = answer;
        }
    }
}