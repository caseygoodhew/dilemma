using System;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    public static class ManualModeration
    {
        private static readonly Lazy<IManualModerationService> _manualModerationService = Locator.Lazy<IManualModerationService>();
        
        public static ModerationViewModel GetNext()
        {
            return _manualModerationService.Value.GetNext();
        }

        public static ModerationViewModel GetNextForUser(int userId)
        {
            return _manualModerationService.Value.GetNextForUser(userId);
        }

        public static ModerationViewModel GetNextForUser(string reference)
        {
            return GetNextForUser(ObjectDictionary.Get<int>(ObjectType.User, reference));
        }

        public static void Approve(int moderationId)
        {
            _manualModerationService.Value.Approve(moderationId);
        }

        public static void Reject(int moderationId, string message)
        {
            _manualModerationService.Value.Reject(moderationId, message);
        }
    }
}
