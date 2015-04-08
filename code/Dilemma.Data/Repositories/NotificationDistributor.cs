using System;
using System.Linq;
using System.Reflection.Emit;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;

using Disposable.Common.ServiceLocator;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    internal class NotificationDistributor : IInternalNotificationDistributor
    {
        private static readonly Lazy<IInternalNotificationRepository> NotificationRepository = Locator.Lazy<IInternalNotificationRepository>();

        private static readonly Lazy<IInternalQuestionRepository> QuestionRepository = Locator.Lazy<IInternalQuestionRepository>();

        /// <summary>
        /// To be called when a question state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{QuestionDataAction}"/>.</param>
        public void OnQuestionStateChange(IMessenger<QuestionDataAction> messenger)
        {
            var messageContext = messenger.GetContext<QuestionMessageContext>(QuestionDataAction.StateChanged);
            var question = messageContext.Question;

            switch (question.QuestionState)
            {
                 case QuestionState.ReadyForModeration:
                    break;
                case QuestionState.Approved:
                    NotificationRepository.Value.Raise(messageContext.DataContext, question.User.UserId, NotificationType.QuestionApproved, NotificationTarget.Questioner, question.QuestionId);
                    break;
                case QuestionState.Rejected:
                    NotificationRepository.Value.Raise(messageContext.DataContext, question.User.UserId, NotificationType.QuestionRejected, NotificationTarget.Questioner, question.QuestionId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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

                    NotificationRepository.Value.Raise(messageContext.DataContext, answer.Question.User.UserId, NotificationType.AnswerApproved, NotificationTarget.Questioner, answer.Question.QuestionId);
                    NotificationRepository.Value.Raise(messageContext.DataContext, answer.User.UserId, NotificationType.AnswerApproved, NotificationTarget.Answerer, answer.Question.QuestionId);
                    ForAllBookmarkers(messageContext.DataContext, answer.Question.QuestionId, NotificationType.AnswerApproved, NotificationTarget.Bookmarker, answer.Question.QuestionId);
                    break;

                case AnswerState.Rejected:
                    NotificationRepository.Value.Raise(messageContext.DataContext, answer.Question.User.UserId, NotificationType.AnswerRejected, NotificationTarget.Answerer, answer.AnswerId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// To be called when an followup state changes.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger{FollowupDataAction}"/>.</param>
        public void OnFollowupStateChange(IMessenger<FollowupDataAction> messenger)
        {
            var messageContext = messenger.GetContext<FollowupMessageContext>(FollowupDataAction.StateChanged);
            var followup = messageContext.Followup;

            switch (followup.FollowupState)
            {
                case FollowupState.ReadyForModeration:
                    break;
                case FollowupState.Approved:
                    NotificationRepository.Value.Raise(messageContext.DataContext, followup.Question.User.UserId, NotificationType.FollowupApproved, NotificationTarget.Questioner, followup.FollowupId);
                    break;
                case FollowupState.Rejected:
                    NotificationRepository.Value.Raise(messageContext.DataContext, followup.Question.User.UserId, NotificationType.FollowupApproved, NotificationTarget.Questioner, followup.Question.QuestionId);
                    ForAllAnswerers(messageContext.DataContext, followup.Question.QuestionId, NotificationType.FollowupApproved, NotificationTarget.Answerer, followup.Question.QuestionId);
                    ForAllBookmarkers(messageContext.DataContext, followup.Question.QuestionId, NotificationType.FollowupApproved, NotificationTarget.Bookmarker, followup.Question.QuestionId);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnVotingOpen(IMessenger<QuestionDataAction> messenger)
        {
            var messageContext = messenger.GetContext<QuestionMessageContext>(QuestionDataAction.OpenForVoting);
            var question = messageContext.Question;

            NotificationRepository.Value.Raise(messageContext.DataContext, question.User.UserId, NotificationType.OpenForVoting, NotificationTarget.Questioner, question.QuestionId);
            ForAllAnswerers(messageContext.DataContext, question.QuestionId, NotificationType.OpenForVoting, NotificationTarget.Answerer, question.QuestionId);
            ForAllBookmarkers(messageContext.DataContext, question.QuestionId, NotificationType.OpenForVoting, NotificationTarget.Bookmarker, question.QuestionId);
        }
        
        public void OnVoteCast(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.VoteCast);
            var answer = messageContext.Answer;

            NotificationRepository.Value.Raise(messageContext.DataContext, answer.User.UserId, NotificationType.VoteOnAnswer, NotificationTarget.Answerer, answer.AnswerId);
        }

        public void OnBestAnswerAwarded(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.BestAnswerAwarded);
            var answer = messageContext.Answer;

            NotificationRepository.Value.Raise(messageContext.DataContext, answer.User.UserId, NotificationType.BestAnswerAwarded, NotificationTarget.Answerer, answer.AnswerId);
            ForAllBookmarkers(messageContext.DataContext, answer.Question.QuestionId, NotificationType.BestAnswerAwarded, NotificationTarget.Bookmarker, answer.AnswerId);
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
            // this should be getting raised before the question state change so that we can inject the Moderation entry into the dictionary? or do this before maybe and kill this method...
            throw new NotImplementedException();
            /*NotificationRepository.Value.Raise(
                dataContext,
                moderation.ForUser.UserId,
                NotificationType.PostRejected,
                moderation.ModerationId);*/
        }

        private static void ForAllAnswerers(DilemmaContext dataContext, int questionId, NotificationType notificationType, NotificationTarget notificationTarget, int id)
        {
            var answerers = QuestionRepository.Value.GetAnswererUserIds(dataContext, questionId);
            answerers.ToList().ForEach(userId => NotificationRepository.Value.Raise(dataContext, userId, notificationType, notificationTarget, id));
        }

        private static void ForAllBookmarkers(DilemmaContext dataContext, int questionId, NotificationType notificationType, NotificationTarget notificationTarget, int id)
        {
            var bookmarkers = QuestionRepository.Value.GetBookmarkUserIds(dataContext, questionId);
            bookmarkers.ToList().ForEach(userId => NotificationRepository.Value.Raise(dataContext, userId, notificationType, notificationTarget, id));
        }
    }
}