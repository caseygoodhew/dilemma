namespace Dilemma.Common
{
    /// <summary>
    /// The type or state of an answer.
    /// </summary>
    public enum AnswerState
    {
        ReservedSlot = 100,
        ReadyForModeration = 200,
        Approved = 300,
        Rejected = 400,
        Expired = 500
    }
}
