using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Models;
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

        private static readonly Lazy<INotificationService> NotificationService = Locator.Lazy<INotificationService>();

        /// <summary>
        /// Gets the next item to be moderated.
        /// </summary>
        /// <returns>The <see cref="ModerationViewModel"/> or null.</returns>
        public ModerationViewModel GetNext()
        {
	        var moderation = ModerationRepository.Value.GetNext<ModerationViewModel>();
	        moderation.ModerationsWaitingCount = ModerationRepository.Value.BacklogCount();
			return moderation;
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

        public QuestionModerationHistoryViewModel GetQuestionHistory(int questionId)
        {
            var moderation =
                ModerationRepository.Value.GetQuestionHistory<QuestionModerationHistoryViewModel>(
                    SecurityManager.Value.GetUserId(),
                    questionId);

            if (moderation != null)
            {
                NotificationService.Value.Mute(NotificationLookupBy.Question, questionId);
            }

            return moderation;
        }

        public AnswerModerationHistoryViewModel GetAnswerHistory(int answerId)
        {
            var moderation =
                ModerationRepository.Value.GetAnswerHistory<AnswerModerationHistoryViewModel>(
                    SecurityManager.Value.GetUserId(),
                    answerId);

            if (moderation != null)
            {
                NotificationService.Value.Mute(NotificationLookupBy.Answer, answerId);
            }

            return moderation;
        }

        public FollowupModerationHistoryViewModel GetFollowupHistory(int followupId)
        {
            var moderation =
                ModerationRepository.Value.GetFollowupHistory<FollowupModerationHistoryViewModel>(
                    SecurityManager.Value.GetUserId(),
                    followupId);

            if (moderation != null)
            {
                NotificationService.Value.Mute(NotificationLookupBy.Followup, followupId);
            }

            return moderation;
        }

        public void ReportQuestion(int questionId, ReportReason reportReason)
        {
            ModerationRepository.Value.ReportQuestion(SecurityManager.Value.GetUserId(), questionId, reportReason);
        }

        public void ReportAnswer(int answerId, ReportReason reportReason)
        {
            ModerationRepository.Value.ReportAnswer(SecurityManager.Value.GetUserId(), answerId, reportReason);
        }

        public void ReportFollowup(int followupId, ReportReason reportReason)
        {
            ModerationRepository.Value.ReportFollowup(SecurityManager.Value.GetUserId(), followupId, reportReason);
        }
    }
}