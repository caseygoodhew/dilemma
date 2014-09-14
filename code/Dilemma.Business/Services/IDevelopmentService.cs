using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    public interface IDevelopmentService
    {
        void SetUserName(int userId, string name);

        IEnumerable<DevelopmentUserViewModel> GetList(IEnumerable<int> userIds);

        DevelopmentUserViewModel Get(int userId);

        bool CanLogin(int userId);
    }
}
