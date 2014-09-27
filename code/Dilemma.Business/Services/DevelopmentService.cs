using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class DevelopmentService : IDevelopmentService
    {
        private static readonly Lazy<IDevelopmentRepository> DevelopmentRepository = Locator.Lazy<IDevelopmentRepository>();
        
        public void SetUserName(int userId, string name)
        {
            DevelopmentRepository.Value.SetUserName(userId, name);
        }

        public IEnumerable<DevelopmentUserViewModel> GetList(IEnumerable<int> userIds)
        {
            return DevelopmentRepository.Value.GetList<DevelopmentUserViewModel>(userIds);
        }

        public DevelopmentUserViewModel Get(int userId)
        {
            return DevelopmentRepository.Value.Get<DevelopmentUserViewModel>(userId);
        }

        public bool CanLogin(int userId)
        {
            return DevelopmentRepository.Value.CanLogin(userId);
        }
    }
}