using System;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// The user service interface.
    /// </summary>
    internal class UserService : IUserService
    {
        private static readonly Lazy<IUserRepository> UserRepository = Locator.Lazy<IUserRepository>();

        private static readonly Lazy<IAdministrationService> AdministrationService = Locator.Lazy<IAdministrationService>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();
        
        /// <summary>
        /// Creates a new anonymous user and returns the user id.
        /// </summary>
        /// <returns>The id of the new user.</returns>
        public int CreateAnonymousUser()
        {
            return UserRepository.Value.CreateAnonymousUser();
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>The user view model.</returns>
        public UserViewModel GetCurrentUser()
        {
            return GetUser(SecurityManager.Value.GetUserId());
        }
        
        /// <summary>
        /// Gets a user.
        /// </summary>
        /// <param name="userId">The user id to get.</param>
        /// <returns>The user view model.</returns>
        public UserViewModel GetUser(int userId)
        {
            var systemServerConfiguration = AdministrationService.Value.GetSystemServerConfiguration();
            var systemEnvironment = systemServerConfiguration.SystemConfigurationViewModel.SystemEnvironment;
            var testingConfiguration = AdministrationService.Value.GetTestingConfiguration();
            var testingConfigurationContext = new TestingConfigurationContext(systemEnvironment, testingConfiguration);
            var serverRole = systemServerConfiguration.ServerConfigurationViewModel.ServerRole;
            var currentUserId = SecurityManager.Value.GetUserId();

            if (   userId == currentUserId 
                || serverRole == ServerRole.Moderation
                || TestingConfiguration.NaturalComparison(testingConfigurationContext, x => x.GetAnyUser).Is(ActiveState.Active))
            {
                return UserRepository.Value.GetUser<UserViewModel>(userId);
            }

            throw new UnauthorizedAccessException("Permission denied retrieving the user with id " + userId);
        }

        public UserStatisticsViewModel GetUserStatistics()
        {
            return UserRepository.Value.GetUserStatistics<UserStatisticsViewModel>(SecurityManager.Value.GetUserId());
        }
    }
}
