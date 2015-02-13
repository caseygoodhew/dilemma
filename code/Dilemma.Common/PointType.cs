using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dilemma.Common
{
    public enum PointType
    {
        [Display(Name = "Question Asked", Description = "Points are awarded for asking a question and contributing to the community. Points are awarded once the question passes moderation.")]
        [DefaultValue(100)]
        QuestionAsked = 1,

        [Display(Name = "Answer Provided", Description = "Points are awarded for answering a question and contributing to the community. Points are awarded once the answer passes moderation.")]
        [DefaultValue(100)]
        AnswerProvided = 2,

        [Display(Name = "Vote Registered", Description = "Points are awarded for voting on a question and contributing to the community. Points are awarded immediately.")]
        [DefaultValue(100)]
        VoteRegistered,

        [Display(Name = "Star Vote Received", Description = "Points are awarded for receiving the star vote on a question and contributing to the community. Points are awarded immediately.")]
        [DefaultValue(100)]
        StarVoteReceived
    }
}
