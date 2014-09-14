using System.Collections.Generic;

namespace Dilemma.Data.Repositories
{
    public interface IDevelopmentRepository
    {
        void SetUserName(int userId, string name);

        IEnumerable<T> GetList<T>(IEnumerable<int> userIds) where T : class;

        T Get<T>(int userId) where T : class;

        bool CanLogin(int userId);
    }
}