using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data
{
    internal class QuestionMessageContext : DataMessageContext<QuestionDataAction>
    {
        public readonly Question Question;

        public QuestionMessageContext(QuestionDataAction messageType, DilemmaContext dataContext, Question question)
            : base(messageType, dataContext)
        {
            Question = question;
        }
    }
}