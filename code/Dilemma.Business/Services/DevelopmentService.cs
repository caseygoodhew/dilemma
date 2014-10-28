using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Development services.
    /// </summary>
    internal class DevelopmentService : IDevelopmentService
    {
        private static readonly Lazy<IDevelopmentRepository> DevelopmentRepository = Locator.Lazy<IDevelopmentRepository>();

        /// <summary>
        /// Sets a name against a user id. This name will not be stored directly against the user.
        /// </summary>
        /// <param name="userId">The id of the user to set the name for.</param>
        /// <param name="name">The user's name.</param>
        public void SetUserName(int userId, string name)
        {
            DevelopmentRepository.Value.SetUserName(userId, name);
        }

        /// <summary>
        /// Gets a list of <see cref="DevelopmentUserViewModel"/>s for the given <see cref="userIds"/>.
        /// </summary>
        /// <param name="userIds">The userIds to get the view models for.</param>
        /// <returns>
        /// The list of <see cref="DevelopmentUserViewModel"/>s. This result is not guaranteed to contain as many entries as were requested. 
        /// This could be because the userIds being requested don't exist any more or the user accounts have been marked as production level accounts.
        /// </returns>
        public IEnumerable<DevelopmentUserViewModel> GetList(IEnumerable<int> userIds)
        {
            return DevelopmentRepository.Value.GetList<DevelopmentUserViewModel>(userIds);
        }

        /// <summary>
        /// Gets a single <see cref="DevelopmentUserViewModel"/>.
        /// </summary>
        /// <param name="userId">The user id of the development user to get.</param>
        /// <returns>The <see cref="DevelopmentUserViewModel"/>.</returns>
        public DevelopmentUserViewModel Get(int userId)
        {
            return DevelopmentRepository.Value.Get<DevelopmentUserViewModel>(userId);
        }

        /// <summary>
        /// Checks if the current user can login as the given <see cref="userId"/>.
        /// </summary>
        /// <param name="userId">The user id that the current user would like to login as.</param>
        /// <returns>True if the current user can login as the requested <see cref="userId"/>.</returns>
        public bool CanLogin(int userId)
        {
            return DevelopmentRepository.Value.CanLoginAs(userId);
        }
    }
}