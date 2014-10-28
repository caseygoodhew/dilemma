using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dilemma.Common
{
    public enum PointType
    {
        [Display(Name = "Question Asked", Description = "Points are awarded for asking a question and contributing to the community. Points are awarded once the question passes moderation.")]
        [DefaultValue(100)]
        QuestionAsked,

        [Display(Name = "Answer Provided", Description = "Points are awarded for answering a question and contributing to the community. Points are awarded once the answer passes moderation.")]
        [DefaultValue(100)]
        AnswerProvided
    }
}
