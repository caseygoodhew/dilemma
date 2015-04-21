namespace Dilemma.Common
{
    /// <summary>
    /// Notification types.
    /// </summary>
    public enum NotificationType
    {
        QuestionApproved = 110,
        AnswerApproved = 120,
        FollowupApproved = 130,

        QuestionRejected = 140,
        AnswerRejected = 150,
        FollowupRejected = 160,
        
        FlaggedQuestionApproved = 170,
        FlaggedAnswerApproved = 180,
        FlaggedFollowupApproved = 190,

        FlaggedQuestionRejected = 200,
        FlaggedAnswerRejected = 210,
        FlaggedFollowupRejected = 220,
        
        OpenForVoting = 230,
        VoteOnAnswer = 240,
        BestAnswerAwarded = 250
    }
}
