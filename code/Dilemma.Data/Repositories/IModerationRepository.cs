namespace Dilemma.Data.Repositories
{
    public interface IModerationRepository
    {
        T GetNext<T>() where T : class;

        void Approve(int userId, int moderationId);

        void Reject(int userId, int moderationId, string message);
    }
}