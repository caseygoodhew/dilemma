using System;
using System.Collections;
using System.Collections.Generic;
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

                    // QuestionApproved -> Questioner
                    NotificationRepository.Value.Raise(
                        messageContext.DataContext,
                        question.User.UserId,
                        NotificationType.QuestionApproved,
                        NotificationTarget.Questioner,
                        NotificationLookupBy.Question,
                        question.QuestionId);

                    break;

                case QuestionState.Rejected:

                    // QuestionRejected -> Questioner
                    NotificationRepository.Value.Raise(
                        messageContext.DataContext,
                        question.User.UserId,
                        NotificationType.QuestionRejected,
                        NotificationTarget.Questioner,
                        NotificationLookupBy.Question,
                        question.QuestionId);

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

                    // AnswerApproved -> Questioner
                    NotificationRepository.Value.Raise(
                        messageContext.DataContext,
                        answer.Question.User.UserId,
                        NotificationType.AnswerApproved,
                        NotificationTarget.Questioner,
                        NotificationLookupBy.Answer,
                        answer.AnswerId);

                    // AnswerApproved -> Answerer
                    NotificationRepository.Value.Raise(
                        messageContext.DataContext,
                        answer.User.UserId,
                        NotificationType.AnswerApproved,
                        NotificationTarget.Answerer,
                        NotificationLookupBy.Answer,
                        answer.AnswerId);

                    var bookmarkers =
                        QuestionRepository.Value.GetBookmarkUserIds(
                            messageContext.DataContext,
                            answer.Question.QuestionId).Where(x => x != answer.User.UserId).ToList();

                    // AnswerApproved -> Bookmarkers
                    bookmarkers.ForEach(
                        userId =>
                        NotificationRepository.Value.Raise(
                            messageContext.DataContext,
                            userId,
                            NotificationType.AnswerApproved,
                            NotificationTarget.Bookmarker,
                            NotificationLookupBy.Answer,
                            answer.AnswerId));

                    break;

                case AnswerState.Rejected:

                    // AnswerRejected -> Answerer
                    NotificationRepository.Value.Raise(
                        messageContext.DataContext,
                        answer.User.UserId,
                        NotificationType.AnswerRejected,
                        NotificationTarget.Answerer,
                        NotificationLookupBy.Answer,
                        answer.AnswerId,
                        false);

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

                    // FollowupApproved -> Questioner
                    NotificationRepository.Value.Raise(
                        messageContext.DataContext,
                        followup.User.UserId,
                        NotificationType.FollowupApproved,
                        NotificationTarget.Questioner,
                        NotificationLookupBy.Followup,
                        followup.FollowupId);

                    var answerers =
                        QuestionRepository.Value.GetAnswererUserIds(
                            messageContext.DataContext,
                            followup.Question.QuestionId).ToList();

                    // FollowupApproved -> Answerers
                    answerers.ForEach(
                        userId =>
                        NotificationRepository.Value.Raise(
                            messageContext.DataContext,
                            userId,
                            NotificationType.FollowupApproved,
                            NotificationTarget.Answerer,
                            NotificationLookupBy.Followup,
                            followup.FollowupId));

                    var bookmarkers =
                        QuestionRepository.Value.GetBookmarkUserIds(
                            messageContext.DataContext,
                            followup.Question.QuestionId).Where(x => !answerers.Contains(x)).ToList();

                    // FollowupApproved -> Bookmarkers
                    bookmarkers.ForEach(
                        userId =>
                        NotificationRepository.Value.Raise(
                            messageContext.DataContext,
                            userId,
                            NotificationType.FollowupApproved, 
                            NotificationTarget.Bookmarker,
                            NotificationLookupBy.Followup,
                            followup.FollowupId));

                    break;

                case FollowupState.Rejected:

                    // FollowupRejected -> Questioner
                    NotificationRepository.Value.Raise(
                        messageContext.DataContext,
                        followup.Question.User.UserId,
                        NotificationType.FollowupRejected,
                        NotificationTarget.Questioner,
                        NotificationLookupBy.Followup,
                        followup.FollowupId,
                        false);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnVotingOpen(IMessenger<QuestionDataAction> messenger)
        {
            var messageContext = messenger.GetContext<QuestionMessageContext>(QuestionDataAction.OpenForVoting);
            var question = messageContext.Question;

            // OpenForVoting -> Questioner
            NotificationRepository.Value.Raise(
                messageContext.DataContext,
                question.User.UserId,
                NotificationType.OpenForVoting,
                NotificationTarget.Questioner,
                NotificationLookupBy.Question,
                question.QuestionId);

            var answerers =
                QuestionRepository.Value.GetAnswererUserIds(messageContext.DataContext, question.QuestionId).ToList();

            // OpenForVoting -> Answerers
            answerers.ForEach(
                userId =>
                NotificationRepository.Value.Raise(
                    messageContext.DataContext,
                    userId,
                    NotificationType.OpenForVoting,
                    NotificationTarget.Answerer,
                    NotificationLookupBy.Question,
                    question.QuestionId));

            var bookmarkers =
                QuestionRepository.Value.GetBookmarkUserIds(messageContext.DataContext, question.QuestionId)
                    .Where(x => !answerers.Contains(x))
                    .ToList();

            // OpenForVoting -> Bookmarkers
            bookmarkers.ForEach(
                userId =>
                NotificationRepository.Value.Raise(
                    messageContext.DataContext,
                    userId,
                    NotificationType.OpenForVoting,
                    NotificationTarget.Bookmarker,
                    NotificationLookupBy.Question,
                    question.QuestionId));
        }
        
        public void OnVoteCast(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.VoteCast);
            var answer = messageContext.Answer;

            // VoteOnAnswer -> Answerers
            NotificationRepository.Value.Raise(
                messageContext.DataContext,
                answer.User.UserId,
                NotificationType.VoteOnAnswer,
                NotificationTarget.Answerer,
                NotificationLookupBy.Answer,
                answer.AnswerId);
        }

        public void OnBestAnswerAwarded(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.BestAnswerAwarded);
            var answer = messageContext.Answer;

            // BestAnswerAwarded -> Questioner
            NotificationRepository.Value.Raise(
                messageContext.DataContext,
                answer.Question.User.UserId,
                NotificationType.BestAnswerAwarded,
                NotificationTarget.Questioner,
                NotificationLookupBy.Answer,
                answer.AnswerId);

            // BestAnswerAwarded -> Answerer
            NotificationRepository.Value.Raise(
                messageContext.DataContext,
                answer.User.UserId,
                NotificationType.BestAnswerAwarded,
                NotificationTarget.Answerer,
                NotificationLookupBy.Answer,
                answer.AnswerId);

            var bookmarkers =
                QuestionRepository.Value.GetBookmarkUserIds(messageContext.DataContext, answer.Question.QuestionId)
                    .Where(x => x != answer.User.UserId)
                    .ToList();

            // BestAnswerAwarded -> Bookmarkers
            bookmarkers.ForEach(
                userId =>
                NotificationRepository.Value.Raise(
                    messageContext.DataContext,
                    userId,
                    NotificationType.BestAnswerAwarded,
                    NotificationTarget.Bookmarker,
                    NotificationLookupBy.Answer,
                    answer.AnswerId));
        }
    }
}