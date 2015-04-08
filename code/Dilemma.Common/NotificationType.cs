namespace Dilemma.Common
{
    /// <summary>
    /// Notification types.
    /// </summary>
    public enum NotificationType
    {
        QuestionApproved,
        AnswerApproved,
        FollowupApproved,

        QuestionRejected,
        AnswerRejected,
        FollowupRejected,
        
        FlaggedQuestionApproved,
        FlaggedAnswerApproved,
        FlaggedFollowupApproved,

        FlaggedQuestionRejected,
        FlaggedAnswerRejected,
        FlaggedFollowupRejected,
        
        OpenForVoting,
        VoteOnAnswer,
        BestAnswerAwarded
    }
}
