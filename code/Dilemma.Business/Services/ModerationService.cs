using System;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class ModerationService : IModerationService
    {
        private static readonly Lazy<IModerationRepository> ModerationRepository = Locator.Lazy<IModerationRepository>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();
        
        public ModerationViewModel GetNext()
        {
            return ModerationRepository.Value.GetNext<ModerationViewModel>();
        }

        public void Approve(int moderationId)
        {
            ModerationRepository.Value.Approve(SecurityManager.Value.GetUserId(), moderationId);
        }

        public void Reject(int moderationId, string message)
        {
            ModerationRepository.Value.Reject(SecurityManager.Value.GetUserId(), moderationId, message);
        }
    }
}