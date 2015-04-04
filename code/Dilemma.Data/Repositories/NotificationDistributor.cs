using System;

using Dilemma.Common;

using Disposable.Common.ServiceLocator;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal class NotificationDistributor : IInternalNotificationDistributor
    {
        private static readonly Lazy<IInternalNotificationRepository> NotificationRepository = Locator.Lazy<IInternalNotificationRepository>();

        /// <summary>
        /// To be called when an answer state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{AnswerDataAction}"/>.</param>
        public void OnAnswerStateChange(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.StateChanged);
            var answer = messageContext.Answer;

            switch (answer.AnswerState)
            {
                case AnswerState.ReservedSlot:
                    break;
                case AnswerState.ReadyForModeration:
                    break;
                case AnswerState.Approved:
                    NotificationRepository.Value.Raise(messageContext.DataContext, answer.Question.User.UserId, NotificationType.QuestionAnswered, answer.AnswerId);
                    break;
                case AnswerState.Rejected:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnFollowupStateChange(IMessenger<FollowupDataAction> messenger)
        {
            var messageContext = messenger.GetContext<FollowupMessageContext>(FollowupDataAction.StateChanged);
            var followup = messageContext.Followup;

            switch (followup.FollowupState)
            {
                case FollowupState.ReadyForModeration:
                    break;
                case FollowupState.Approved:
                    NotificationRepository.Value.Raise(messageContext.DataContext, followup.Question.User.UserId, NotificationType.QuestionFollowuped, followup.FollowupId);
                    break;
                case FollowupState.Rejected:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// To be called when a moderation state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{ModerationState}"/>.</param>
        public void OnModerationRejected(IMessenger<ModerationState> messenger)
        {
            var messageContext = messenger.GetContext<ModerationMessageContext>(ModerationState.Rejected);
            var dataContext = messageContext.DataContext;
            var moderation = messageContext.Moderation;

            NotificationRepository.Value.Raise(
                dataContext,
                moderation.ForUser.UserId,
                NotificationType.PostRejected,
                moderation.ModerationId);
        }
    }
}