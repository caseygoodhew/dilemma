using System;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Moderation service.
    /// </summary>
    internal class ManualModerationService : IManualModerationService
    {
        private static readonly Lazy<IManualModerationRepository> ModerationRepository = Locator.Lazy<IManualModerationRepository>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        /// <summary>
        /// Gets the next item to be moderated.
        /// </summary>
        /// <returns>The <see cref="ModerationViewModel"/> or null.</returns>
        public ModerationViewModel GetNext()
        {
            return ModerationRepository.Value.GetNext<ModerationViewModel>();
        }

        /// <summary>
        /// Gets the next item to be moderated for the specified user.
        /// </summary>
        /// <param name="userId">The user id to get the next moderation for.</param>
        /// <returns>The <see cref="ModerationViewModel"/> or null.</returns>
        public ModerationViewModel GetNextForUser(int userId)
        {
            return ModerationRepository.Value.GetNextForUser<ModerationViewModel>(userId);
        }

        /// <summary>
        /// Marks a moderation item as being approved.
        /// </summary>
        /// <param name="moderationId">The moderation id to approve.</param>
        public void Approve(int moderationId)
        {
            ModerationRepository.Value.Approve(SecurityManager.Value.GetUserId(), moderationId);
        }

        /// <summary>
        /// Marks a moderation item as being rejected.
        /// </summary>
        /// <param name="moderationId">The moderation id to reject.</param>
        /// <param name="message">A message detailing the reason for rejection.</param>
        public void Reject(int moderationId, string message)
        {
            ModerationRepository.Value.Reject(SecurityManager.Value.GetUserId(), moderationId, message);
        }
    }
}