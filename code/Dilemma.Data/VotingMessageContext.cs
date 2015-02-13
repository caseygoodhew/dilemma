using Dilemma.Data.EntityFramework;

namespace Dilemma.Data
{
    internal class VotingMessageContext : DataMessageContext<VotingDataAction>
    {
        public readonly int UserId;

        public readonly int QuestionId;

        public readonly int QuestionUserId;

        public readonly int AnswerId;

        public readonly int AnswerUserId;

        public VotingMessageContext(VotingDataAction messageType, DilemmaContext dataContext, int userId, int questionId, int questionUserId, int answerId, int answerUserId)
            : base(messageType, dataContext)
        {
            UserId = userId;
            QuestionId = questionId;
            QuestionUserId = questionUserId;
            AnswerId = answerId;
            AnswerUserId = answerUserId;
        }
    }
}