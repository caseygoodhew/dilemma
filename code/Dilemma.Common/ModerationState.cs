namespace Dilemma.Common
{
    /// <summary>
    /// Moderation states.
    /// </summary>
    public enum ModerationState
    {
        Queued = 100,
        Approved = 200,
        Rejected = 300,
        Reported = 400,
        ReQueued = 500
    }
}