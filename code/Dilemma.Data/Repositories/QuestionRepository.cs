﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Question repository implementation.
    /// </summary>
    internal class QuestionRepository : IInternalQuestionRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        private static readonly Lazy<IMessagePipe<QuestionDataAction>> QuestionMessagePipe = Locator.Lazy<IMessagePipe<QuestionDataAction>>();
        
        private static readonly Lazy<IMessagePipe<AnswerDataAction>> AnswerMessagePipe = Locator.Lazy<IMessagePipe<AnswerDataAction>>();
        
        /// <summary>
        /// Creates a <see cref="Question"/> from the specified type. There must be a converter registered between <see cref="T"/> and <see cref="Question"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user creating the question.</param>
        /// <param name="questionType">The convertable instance.</param>
        /// <returns>The new question id.</returns>
        public int CreateQuestion<T>(int userId, T questionType) where T : class
        {
            var question = ConverterFactory.ConvertOne<T, Question>(questionType);

            using (var context = new DilemmaContext())
            {
                question.User = context.GetOrAttachNew<User, int>(userId, x => x.UserId);
                context.EnsureAttached(question.Category, x => x.CategoryId);
                
                question.QuestionState = QuestionState.Approved;

                context.Questions.Add(question);
                context.SaveChangesVerbose();

                var messageContext = new QuestionMessageContext(QuestionDataAction.Created, context, question);
                QuestionMessagePipe.Value.Announce(messageContext);
            }

            return question.QuestionId;
        }

        /// <summary>
        /// Gets the <see cref="Question"/> in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The user id of the user requesting the question.</param>
        /// <param name="questionId">The id of the question to get.</param>
        /// <param name="config">Options for what extended data to include.</param>
        /// <returns>The <see cref="Question"/> converted to type T.</returns>
        public T GetQuestion<T>(int userId, int questionId, GetQuestionAs config) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return GetQuestion<T>(context, userId, questionId, config);
            }
        }

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="userId">The id of user to get the activity for. If this is null, all questions will be returned.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        public IEnumerable<T> QuestionList<T>(int? userId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var questions =
                    context.Questions.Include(x => x.Category)
                        .Where(
                            x =>
                            !userId.HasValue || x.User.UserId == userId.Value
                            || x.Answers.Any(a => a.User.UserId == userId.Value))
                        .Select(
                            x =>
                            new
                                {
                                    x.QuestionId,
                                    TotalAnswer = x.Answers.Count(y => y.AnswerState != AnswerState.Rejected),
                                    x.MaxAnswers,
                                    x.Category,
                                    x.Text,
                                    x.ClosesDateTime,
                                    x.CreatedDateTime,
                                    x.User.UserId,
                                    x.QuestionState,
                                    MostRecentActivity = x.Answers.Where(a => a.AnswerState == AnswerState.Approved).Select(a => a.CreatedDateTime).Concat(new[] { x.CreatedDateTime }).Max()
                                })
                        .AsEnumerable()
                        .OrderByDescending(x => x.MostRecentActivity)
                        .Select(
                            x =>
                            new Question
                                {
                                    QuestionId = x.QuestionId,
                                    TotalAnswers = x.TotalAnswer,
                                    MaxAnswers = x.MaxAnswers,
                                    Category = x.Category,
                                    Text = x.Text,
                                    ClosesDateTime = x.ClosesDateTime,
                                    CreatedDateTime = x.CreatedDateTime,
                                    QuestionState = x.QuestionState,
                                    User = new User { UserId = x.UserId }
                                });

                return ConverterFactory.ConvertMany<Question, T>(questions.ToList());
            }
        }

        /// <summary>
        /// Requests an an slot for the given question id. If no slot is available, null is returned.
        /// </summary>
        /// <param name="userId">The id of the user requesting the answer slot.</param>
        /// <param name="questionId">The question id.</param>
        /// <returns>The answer id if a slot is available or null if it is not available.</returns>
        public int? RequestAnswerSlot(int userId, int questionId)
        {
            using (var context = new DilemmaContext())
            {
                var existingAnswer = GetAnswerInProgress(context, userId, questionId, null);

                if (existingAnswer != null)
                {
                    return existingAnswer.AnswerId;
                }
                
                var question = GetQuestion<Question>(context, userId, questionId, GetQuestionAs.AnswerCount);

                if (question.User.UserId == userId || question.QuestionState != QuestionState.Approved || question.TotalAnswers >= question.MaxAnswers || question.ClosesDateTime < TimeSource.Value.Now)
                {
                    return null;
                }

                var answer = new Answer
                                 {
                                     CreatedDateTime = TimeSource.Value.Now,
                                     Question = question,
                                     User = new User { UserId = userId }
                                 };
                
                context.EnsureAttached(question, x => x.QuestionId);
                context.EnsureAttached(answer.User, x => x.UserId);
                
                // TODO: Potential concurrency issue here if two people vie for the slot at the same time.
                // TODO:    (The worst that will happen is that more answer slots will be assigned than allowed)
                context.Answers.Add(answer);
                context.SaveChangesVerbose();

                return answer.AnswerId;
            }
        }

        /// <summary>
        /// Gets an answer in progress by question id and answer id. The provided answer id must be an answer of the question id.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="userId">The id of the user who is requesting the answer.</param>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerId">The answer id.</param>
        /// <returns>The <see cref="Answer"/> converted to type T.</returns>
        public T GetAnswerInProgress<T>(int userId, int questionId, int answerId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return ConverterFactory.ConvertOne<Answer, T>(GetAnswerInProgress(context, userId, questionId, answerId));
            }
        }

        /// <summary>
        /// Completes an answer that is in an initial 'Answer slot' state.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user who is completing the answer.</param>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerType">The convertable instance.</param>
        /// <returns>true if the answer was saved, false if the answer slot was no longer available.</returns>
        public bool CompleteAnswer<T>(int userId, int questionId, T answerType) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var answer = ConverterFactory.ConvertOne<T, Answer>(answerType);

                var existingAnswer = GetAnswerInProgress(context, userId, questionId, answer.AnswerId);

                if (existingAnswer == null)
                {
                    return false;
                }

                existingAnswer.CreatedDateTime = TimeSource.Value.Now;
                existingAnswer.AnswerState = AnswerState.Approved;
                existingAnswer.Text = answer.Text;

                context.Answers.Update(context, existingAnswer);
                context.SaveChangesVerbose();

                var messageContext = new AnswerMessageContext(AnswerDataAction.Created, context, existingAnswer);
                AnswerMessagePipe.Value.Announce(messageContext);

                return true;
            }
        }

        /// <summary>
        /// To be called when the moderation state is updated.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public void OnModerationStateUpdated(IMessenger<ModerationState> messenger)
        {
            var messageContext = messenger.GetContext<ModerationMessageContext>(EnumExtensions.GetValues<ModerationState>());
            var moderation = messageContext.Moderation;
            var moderationEntry = messageContext.ModerationEntry;
            var dataContext = messageContext.DataContext;

            switch (moderation.ModerationFor)
            {
                case ModerationFor.Question:
                    UpdateQuestionState(dataContext, moderation.Question.QuestionId, moderationEntry.State);
                    break;
                case ModerationFor.Answer:
                    UpdateAnswerState(dataContext, moderation.Answer.AnswerId, moderationEntry.State);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            messenger.Forward();
        }

        private void UpdateAnswerState(DilemmaContext dataContext, int answerId, ModerationState moderationState)
        {
            var answer = dataContext.Answers.Include(x => x.Question).Include(x => x.Question.User).Single(x => x.AnswerId == answerId);
            AnswerState newAnswerState;
            
            
            switch (moderationState)
            {
                case ModerationState.Queued:
                    newAnswerState = AnswerState.ReadyForModeration;
                    break;
                case ModerationState.Approved:
                    newAnswerState = AnswerState.Approved;
                    break;
                case ModerationState.Rejected:
                    newAnswerState = AnswerState.Rejected;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("moderationState");
            }

            if (newAnswerState == answer.AnswerState)
            {
                return;
            }

            answer.AnswerState = newAnswerState;
            dataContext.Answers.Update(dataContext, answer);
            dataContext.SaveChangesVerbose();

            var messageContext = new AnswerMessageContext(AnswerDataAction.StateChanged, dataContext, answer);
            AnswerMessagePipe.Value.Announce(messageContext);
        }

        private void UpdateQuestionState(DilemmaContext dataContext, int questionId, ModerationState moderationState)
        {
            var question = dataContext.Questions.Single(x => x.QuestionId == questionId);

            switch (moderationState)
            {
                case ModerationState.Queued:
                    question.QuestionState = QuestionState.ReadyForModeration;
                    break;
                case ModerationState.Approved:
                    question.QuestionState = QuestionState.Approved;
                    break;
                case ModerationState.Rejected:
                    question.QuestionState = QuestionState.Rejected;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("moderationState");
            }

            dataContext.Questions.Update(dataContext, question);
            dataContext.SaveChangesVerbose();

            var messageContext = new QuestionMessageContext(QuestionDataAction.StateChanged, dataContext, question);
            QuestionMessagePipe.Value.Announce(messageContext);
        }

        private static T GetQuestion<T>(DilemmaContext context, int userId, int questionId, GetQuestionAs config) where T : class
        {
            Question question;

            switch (config)
            {
                case GetQuestionAs.AnswerCount:
                    question =
                        context.Questions.Where(x => x.QuestionId == questionId)
                            .Select(x => new
                                             {
                                                 x.QuestionId,
                                                 x.MaxAnswers,
                                                 x.ClosesDateTime,
                                                 x.User.UserId,
                                                 TotalAnswers = x.Answers.Count(y => y.AnswerState != AnswerState.Rejected),
                                                 x.QuestionState
                                             })
                            .AsEnumerable()
                            .Select(x => new Question
                                             {
                                                 QuestionId = x.QuestionId,
                                                 MaxAnswers = x.MaxAnswers,
                                                 ClosesDateTime = x.ClosesDateTime,
                                                 User = new User { UserId = x.UserId },
                                                 TotalAnswers = x.TotalAnswers,
                                                 QuestionState = x.QuestionState
                                             })
                            .Single();

                    break;

                case GetQuestionAs.FullDetails:

                    question =
                        context.Questions
                            .Where(x => x.QuestionId == questionId)
                            .Where(x => x.QuestionState == QuestionState.Approved || x.User.UserId == userId)
                            .Include(x => x.User)
                            .Include(x => x.Category)
                            .Include(x => x.Answers)
                            .Include(x => x.Answers.Select(a => a.User))
                            .Select(
                                x => new
                                         {
                                             x.QuestionId,
                                             x.Category,
                                             x.ClosesDateTime,
                                             x.CreatedDateTime,
                                             x.MaxAnswers,
                                             x.QuestionState,
                                             x.Text,
                                             TotalAnswers = x.Answers.Count(y => y.AnswerState != AnswerState.Rejected),
                                             Answers = x.Answers.Where(a => a.AnswerState == AnswerState.Approved || a.User.UserId == userId).Select(
                                                a => new
                                                {
                                                    a.AnswerId,
                                                    a.AnswerState,
                                                    a.CreatedDateTime,
                                                    a.Text,
                                                    a.User
                                                }
                                             ),
                                             x.User
                                         })
                            .AsEnumerable()
                            .Select(
                                x => new Question
                                        {
                                            Answers = x.Answers.Select(a => new Answer
                                            {
                                                AnswerId = a.AnswerId,
                                                AnswerState = a.AnswerState,
                                                CreatedDateTime = a.CreatedDateTime,
                                                Text = a.Text,
                                                User = a.User
                                            }).ToList(),
                                            Category = x.Category,
                                            ClosesDateTime = x.ClosesDateTime,
                                            CreatedDateTime = x.ClosesDateTime,
                                            MaxAnswers = x.MaxAnswers,
                                            QuestionId = x.QuestionId,
                                            QuestionState = x.QuestionState,
                                            Text = x.Text,
                                            TotalAnswers = x.TotalAnswers,
                                            User = x.User
                                        })
                             .SingleOrDefault();

                    if (question != null)
                    {
                        question.Answers.ForEach(x => x.Question = question);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ConverterFactory.ConvertOne<Question, T>(question);
        }

        private static Answer GetAnswerInProgress(DilemmaContext context, int userId, int questionId, int? answerId)
        {
            var query =
                context.Answers.AsNoTracking()
                    .Include(x => x.Question)
                    .Include(x => x.Question.User)
                    .Include(x => x.User)
                    .Where(x => x.Question.QuestionId == questionId)
                    .Where(x => x.User.UserId == userId)
                    .Where(x => x.AnswerState == AnswerState.ReservedSlot);

            if (answerId.HasValue)
            {
                query = query.Where(x => x.AnswerId == answerId);
            }

            return query.FirstOrDefault();
        }
    }
}
