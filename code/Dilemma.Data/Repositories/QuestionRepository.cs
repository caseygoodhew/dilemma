﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Xml;

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

        private static readonly Lazy<IInternalUserRepository> UserRepository = Locator.Lazy<IInternalUserRepository>();
        
        private static readonly Lazy<IMessagePipe<QuestionDataAction>> QuestionMessagePipe = Locator.Lazy<IMessagePipe<QuestionDataAction>>();
        
        private static readonly Lazy<IMessagePipe<AnswerDataAction>> AnswerMessagePipe = Locator.Lazy<IMessagePipe<AnswerDataAction>>();

        private static readonly Lazy<IMessagePipe<FollowupDataAction>> FollowupMessagePipe = Locator.Lazy<IMessagePipe<FollowupDataAction>>();

        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();

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
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        public IEnumerable<T> QuestionList<T>(int maximum) where T : class
        {
            /////////////////////////////////////////////////////////////////
            // TODO: THIS SHOULD NOT GET CALLED EVERY TIME
            CloseQuestions();
            AdministrationRepository.Value.RetireOldQuestions();
            /////////////////////////////////////////////////////////////////
            
            using (var context = new DilemmaContext())
            {
                return QuestionList<T>(context.Questions, maximum);
            }
        }

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <param name="userId">The id of user to get the questions for.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        public IEnumerable<T> QuestionList<T>(int userId, int maximum) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return
                    QuestionList<T>(
                        context.Questions.Where(
                            x =>
                            x.User.UserId == userId
                            || x.Answers.Any(a => a.User.UserId == userId && a.AnswerState != AnswerState.Rejected)),
                        maximum);
            }
        }

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <typeparam name="TC">The <see cref="category"/> type.</typeparam>
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <param name="category">The category of question to get.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        public IEnumerable<T> QuestionList<T, TC>(TC category, int maximum) where T : class where TC : class
        {
            var categoryId = ConverterFactory.ConvertOne<TC, Category>(category).CategoryId;
            
            using (var context = new DilemmaContext())
            {
                return QuestionList<T>(context.Questions.Where(x => x.Category.CategoryId == categoryId), maximum);
            }
        }

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <param name="questionIds">The ids of question to get.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        public IEnumerable<T> QuestionList<T>(IEnumerable<int> questionIds, int maximum) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return QuestionList<T>(context.Questions.Where(x => questionIds.Contains(x.QuestionId)), maximum);
            }
        }

        private static IEnumerable<T> QuestionList<T>(IQueryable<Question> baseQuery, int maximum) where T : class
        {
            var questions = baseQuery.Where(x => x.QuestionState != QuestionState.Rejected)
                .OrderByDescending(x => Guid.NewGuid())
                .Take(maximum)
                .Select(
                    x => new
                             {
                                 x.QuestionId,
                                 TotalAnswer = x.Answers.Count(y => y.AnswerState != AnswerState.Rejected),
                                 x.MaxAnswers,
                                 x.Category,
                                 x.Text,
                                 x.ClosesDateTime,
                                 x.ClosedDateTime,
                                 x.CreatedDateTime,
                                 x.User.UserId,
                                 x.QuestionState,
                                 MostRecentActivity = x.Answers.Where(a => a.AnswerState == AnswerState.Approved).Select(a => a.CreatedDateTime).Concat(new[] { x.CreatedDateTime }).Max()
                             })
                .AsEnumerable()
                .OrderByDescending(x => x.MostRecentActivity)
                .Select(
                    x => new Question
                            {
                                QuestionId = x.QuestionId,
                                TotalAnswers = x.TotalAnswer,
                                MaxAnswers = x.MaxAnswers,
                                Category = x.Category,
                                Text = x.Text,
                                ClosesDateTime = x.ClosesDateTime,
                                ClosedDateTime = x.ClosedDateTime,
                                CreatedDateTime = x.CreatedDateTime,
                                QuestionState = x.QuestionState,
                                User = new User { UserId = x.UserId }
                            });

            return ConverterFactory.ConvertMany<Question, T>(questions.ToList());
        }

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="userId">The id of user to get the activity for. If this is null, all questions will be returned.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        public IEnumerable<T> QuestionList<T>(int? userId) where T : class
        {
            // Only allow unrestricted "GetAll" when we're on an internal environment. 
            // The production environment could burn with a long running blocking query.
            var systemConfiguration = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();

            if (!SystemEnvironmentValidation.IsInternalEnvironment(systemConfiguration.SystemEnvironment))
            {
                return Enumerable.Empty<T>();
            }
            
            using (var context = new DilemmaContext())
            {
                var questions =
                    context.Questions.Include(x => x.Category)
                        .Where(x => !userId.HasValue 
                            || x.User.UserId == userId.Value
                            || x.Answers.Any(a => a.User.UserId == userId.Value && a.AnswerState != AnswerState.Rejected))
                        .Where(x => x.QuestionState != QuestionState.Rejected)
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
                                    x.ClosedDateTime,
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
                                    ClosedDateTime = x.ClosedDateTime,
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
            /////////////////////////////////////////////////////////////////
            // TODO: THIS SHOULD NOT GET CALLED EVERY TIME
            AdministrationRepository.Value.ExpireAnswerSlots();
            /////////////////////////////////////////////////////////////////
            
            using (var context = new DilemmaContext())
            {
                if (HasUserAnsweredQuestion(context, userId, questionId))
                {
                    return null;
                }
                
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
                                     LastTouchedDateTime = TimeSource.Value.Now,
                                     Question = question,
                                     User = new User { UserId = userId },
                                     AnswerState = AnswerState.ReservedSlot
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
        /// Touches an answer so that the answer slot does not expire.
        /// </summary>
        /// <param name="userId">The user who is touching the answer.</param>
        /// <param name="answerId">The id of the answer to touch.</param>
        public void TouchAnswer(int userId, int answerId)
        {
            using (var context = new DilemmaContext())
            {
                var answer =
                    context.Answers
                            .Where(x => x.User.UserId == userId)
                            .SingleOrDefault(x => x.AnswerId == answerId);

                if (answer == null)
                {
                    return;
                }

                answer.LastTouchedDateTime = TimeSource.Value.Now;

                context.Answers.Update(context, answer);
                context.SaveChangesVerbose();
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
            var answer = ConverterFactory.ConvertOne<T, Answer>(answerType);
            
            using (var context = new DilemmaContext())
            {
                var existingAnswer = GetAnswerInProgress(context, userId, questionId, answer.AnswerId);

                if (existingAnswer == null)
                {
                    return false;
                }

                existingAnswer.LastTouchedDateTime = TimeSource.Value.Now;
                existingAnswer.AnswerState = AnswerState.Approved;
                existingAnswer.Text = answer.Text;
                existingAnswer.WebPurifyAttempted = answer.WebPurifyAttempted;
                existingAnswer.PassedWebPurify = answer.PassedWebPurify;

                context.Answers.Update(context, existingAnswer);
                context.SaveChangesVerbose();

                var messageContext = new AnswerMessageContext(AnswerDataAction.Created, context, existingAnswer);
                AnswerMessagePipe.Value.Announce(messageContext);

                return true;
            }
        }

        /// <summary>
        /// Adds a followup to a question.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user who is completing the followup (this must be the question owner).</param>
        /// <param name="questionId">The question id.</param>
        /// <param name="followupType">The convertable instance.</param>
        /// <returns>true if the followup was saved, false if a followup has already been added.</returns>
        public bool AddFollowup<T>(int userId, int questionId, T followupType) where T : class
        {
            var followup = ConverterFactory.ConvertOne<T, Followup>(followupType);

            using (var context = new DilemmaContext())
            {
                var question =
                    context.Questions.Where(x => x.QuestionId == questionId)
                        .Where(x => x.User.UserId == userId)
                        .Include(x => x.Followup)
                        .AsNoTracking()
                        .Select(
                            x => new
                                     {
                                         x.QuestionId,
                                         x.QuestionState,
                                         x.ClosedDateTime,
                                         x.Followup
                                     }).ToList().Select(
                                         x => new Question
                                                  {
                                                      QuestionId = x.QuestionId,
                                                      QuestionState = x.QuestionState,
                                                      ClosedDateTime = x.ClosedDateTime,
                                                      Followup = x.Followup
                                                  }).SingleOrDefault();

                if (question == null 
                    || question.ClosedDateTime == null 
                    || question.Followup != null
                    || question.QuestionState != QuestionState.Approved)
                {
                    return false;
                }

                followup.User = context.GetOrAttachNew<User, int>(userId, x => x.UserId);
                followup.Question = context.GetOrAttachNew<Question, int>(questionId, x => x.QuestionId);
                followup.Question.Followup = followup;
                followup.CreatedDateTime = TimeSource.Value.Now;
                followup.FollowupState = FollowupState.Approved;
                
                context.Followups.Add(followup);
                context.SaveChangesVerbose();

                var messageContext = new FollowupMessageContext(FollowupDataAction.Created, context, followup);
                FollowupMessagePipe.Value.Announce(messageContext);
            }

            return true;
        }

        /// <summary>
        /// Registers a users vote for an answer.
        /// </summary>
        /// <param name="userId">The user id of the user that is voting.</param>
        /// <param name="answerId">The answer to register the vote against.</param>
        public void RegisterVote(int userId, int answerId)
        {
            using (var context = new DilemmaContext())
            {
                var result =
                    context.Answers
                        .AsNoTracking()
                        .Where(x => x.AnswerId == answerId)
                        .Where(x => x.AnswerState == AnswerState.Approved)
                        .Where(x => x.Question.QuestionState == QuestionState.Approved)
                        .Where(x => x.Question.ClosedDateTime != null)
                        .Select(
                            x => new
                                     {
                                         QuestionId = x.Question.QuestionId,
                                         QuestionUserId = x.Question.User.UserId
                                     }).SingleOrDefault();

                if (result == null)
                {
                    return;
                }

                var existingVote =
                    context.Vote.Where(x => x.User.UserId == userId)
                        .Where(
                            x =>
                               x.Answer.AnswerId == answerId
                            || x.Question.QuestionId == result.QuestionId);

                if (existingVote.Any())
                {
                    return;
                }

                var answer = context.Answers.Include(x => x.User).Include(x => x.Question).Include(x => x.Question.User).Single(x => x.AnswerId == answerId);
                
                var vote = new Vote
                                   {
                                       CreatedDateTime = TimeSource.Value.Now,
                                       User = context.GetOrAttachNew<User, int>(userId, x => x.UserId),
                                       Question = context.GetOrAttachNew<Question, int>(result.QuestionId, x => x.QuestionId),
                                       Answer = answer
                                   };

                context.Vote.Add(vote);

                context.SaveChangesVerbose();

                var answerDataAction = userId == result.QuestionUserId
                                           ? AnswerDataAction.BestAnswerAwarded
                                           : AnswerDataAction.VoteCast;

                var messageContext = new AnswerMessageContext(answerDataAction, context, vote.Answer);
                messageContext.Dictionary["VoteCastBy"] = userId;

                AnswerMessagePipe.Value.Announce(messageContext);
            }
        }

        /// <summary>
        /// Deregisters a users vote for an answer.
        /// </summary>
        /// <param name="userId">The user id of the user that is deregistering their vote.</param>
        /// <param name="answerId">The answer to deregister the vote against.</param>
        public void DeregisterVote(int userId, int answerId)
        {
            // this is not required functionality at this time
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the user id of the user who posted the question.
        /// </summary>
        /// <param name="answerId">The answer id of the question to find.</param>
        /// <returns>The user id of the user who posted the questions.</returns>
        public int GetQuestionUserIdByAnswerId(int answerId)
        {
            using (var context = new DilemmaContext())
            {
                return
                    context.Answers.Where(x => x.AnswerId == answerId)
                        .Select(x => x.Question.User.UserId)
                        .SingleOrDefault();
            }
        }

        /// <summary>
        /// Bookmarks a question
        /// </summary>
        /// <param name="userId">The user id of the user that is bookmarking.</param>
        /// <param name="questionId">The question to bookmark.</param>
        public void AddBookmark(int userId, int questionId)
        {
            using (var context = new DilemmaContext())
            {
                var result = context.Bookmarks.Where(x => x.User.UserId == userId)
                    .FirstOrDefault(x => x.Question.QuestionId == questionId);

                if (result != null)
                {
                    return;
                }

                var bookmark = new Bookmark
                        {
                            Question = new Question { QuestionId = questionId },
                            User = new User { UserId = userId },
                            CreatedDateTime = TimeSource.Value.Now
                        };

                context.EnsureAttached(bookmark.Question, x => x.QuestionId);
                context.EnsureAttached(bookmark.User, x => x.UserId);

                context.Bookmarks.Add(bookmark);

                context.SaveChangesVerbose();
            }
        }

        /// <summary>
        /// Removes a question bookmark
        /// </summary>
        /// <param name="userId">The user id of the user that is removing the bookmark.</param>
        /// <param name="questionId">The question to remove the bookmark from.</param>
        public void RemoveBookmark(int userId, int questionId)
        {
            using (var context = new DilemmaContext())
            {
                var result =
                    context.Bookmarks
                        .Where(x => x.User.UserId == userId)
                        .Where(x => x.Question.QuestionId == questionId)
                        .ToList();

                context.Bookmarks.RemoveRange(result);

                context.SaveChangesVerbose();
            }
        }

        public IEnumerable<int> GetBookmarkedQuestionIds(int userId)
        {
            using (var context = new DilemmaContext())
            {
                return context.Bookmarks.Where(x => x.User.UserId == userId).Select(x => x.Question.QuestionId).ToList();
            }
        }

		public void CloseQuestions()
        {
            using (var context = new DilemmaContext())
            {
	            var now = TimeSource.Value.Now;
				
				var questionSetOne =
		            context.Questions.Include(x => x.User).Where(x => x.QuestionState == Common.QuestionState.Approved)
			            .Where(x => x.ClosedDateTime == null)
			            .Where(x => x.ClosesDateTime < now)
			            .ToList();

	            var questionSetTwo =
		            context.Questions.Include(x => x.User).Where(x => x.QuestionState == QuestionState.Approved)
			            .Where(x => x.ClosedDateTime == null)
			            .Where(x => x.MaxAnswers <= x.Answers.Count(a => a.AnswerState == Common.AnswerState.Approved))
			            .ToList();

	            var questions = questionSetOne.Concat(questionSetTwo).GroupBy(x => x.QuestionId).Select(x => x.First()).ToList();

	            foreach (var question in questions)
	            {
		            var messageContext = new QuestionMessageContext(QuestionDataAction.OpenForVoting, context, question);
		            QuestionMessagePipe.Value.Announce(messageContext);
	            }

				questions.ForEach(x => x.ClosedDateTime = now);

                var lastRunLog = context.LastRunLog.Single();
                lastRunLog.CloseQuestions = TimeSource.Value.Now;
                context.Entry(lastRunLog).Property(x => x.CloseQuestions).IsModified = true;

	            context.SaveChangesVerbose();
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
                case ModerationFor.Followup:
                    UpdateFollowupState(dataContext, moderation.Followup.FollowupId, moderationEntry.State);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            messenger.Forward();
        }

        public IEnumerable<int> GetBookmarkUserIds(DilemmaContext dataContext, int questionId)
        {
            return
                dataContext.Bookmarks.Where(x => x.Question.QuestionId == questionId)
                    .AsNoTracking()
                    .Select(x => x.User.UserId)
                    .Distinct()
                    .ToList();
        }

        public IEnumerable<int> GetAnswererUserIds(DilemmaContext dataContext, int questionId)
        {
            return
                dataContext.Answers.Where(x => x.Question.QuestionId == questionId)
                    .AsNoTracking()
                    .Select(x => x.User.UserId)
                    .Distinct()
                    .ToList();
        }

        private void UpdateAnswerState(DilemmaContext dataContext, int answerId, ModerationState moderationState)
        {
            var answer = dataContext.Answers.Include(x => x.User).Include(x => x.Question).Include(x => x.Question.User).Single(x => x.AnswerId == answerId);
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

        private void UpdateFollowupState(DilemmaContext dataContext, int followupId, ModerationState moderationState)
        {
            var followup = dataContext.Followups.Include(x => x.Question).Include(x => x.Question.User).Single(x => x.FollowupId == followupId);
            FollowupState newFollowupState;
            
            switch (moderationState)
            {
                case ModerationState.Queued:
                    newFollowupState = FollowupState.ReadyForModeration;
                    break;
                case ModerationState.Approved:
                    newFollowupState = FollowupState.Approved;
                    break;
                case ModerationState.Rejected:
                    newFollowupState = FollowupState.Rejected;
                    followup.Question.Followup = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("moderationState");
            }

            if (newFollowupState == followup.FollowupState)
            {
                return;
            }

            followup.FollowupState = newFollowupState;
            
            dataContext.Followups.Update(dataContext, followup);
            dataContext.SaveChangesVerbose();

            var messageContext = new FollowupMessageContext(FollowupDataAction.StateChanged, dataContext, followup);
            FollowupMessagePipe.Value.Announce(messageContext);
        }
        
        private void UpdateQuestionState(DilemmaContext dataContext, int questionId, ModerationState moderationState)
        {
            QuestionState newQuestionState;
            
            switch (moderationState)
            {
                case ModerationState.Queued:
                    newQuestionState = QuestionState.ReadyForModeration;
                    break;
                case ModerationState.Approved:
                    newQuestionState = QuestionState.Approved;
                    break;
                case ModerationState.Rejected:
                    newQuestionState = QuestionState.Rejected;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("moderationState");
            }
            
            var question = dataContext.Questions.Include(x => x.User).Single(x => x.QuestionId == questionId);

            if (newQuestionState == question.QuestionState)
            {
                return;
            }

            question.QuestionState = newQuestionState;
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
                                                 x.ClosedDateTime,
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
                                                 ClosedDateTime = x.ClosedDateTime,
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
                            .Include(x => x.Followup)
                            .Include(x => x.Followup.User)
                            .Select(
                                x => new
                                         {
                                             x.QuestionId,
                                             x.Category,
                                             x.ClosesDateTime,
                                             x.CreatedDateTime,
                                             x.ClosedDateTime,
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
                                             x.User,
                                             Followup = x.Followup != null && (x.Followup.FollowupState == FollowupState.Approved || x.Followup.User.UserId == userId) ? x.Followup : null
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
											CreatedDateTime = x.CreatedDateTime,
                                            ClosedDateTime = x.ClosedDateTime,
                                            MaxAnswers = x.MaxAnswers,
                                            QuestionId = x.QuestionId,
                                            QuestionState = x.QuestionState,
                                            Text = x.Text,
                                            TotalAnswers = x.TotalAnswers,
                                            User = x.User,
                                            Followup = x.Followup
                                        })
                             .SingleOrDefault();

                    if (question != null)
                    {
                        // get all users involved
                        var users = question.Answers.Select(x => x.User).Add(question.User).ToList();
                        
                        var userTotalPoints = UserRepository.Value.GetTotalUserPoints(context, users.Select(x => x.UserId));
                        
                        // set the total points for each of the answer users
                        users.ForEach(
                            x =>
                                {
                                    x.TotalPoints = userTotalPoints[x.UserId];
                                });

                        var voteCounts = GetVoteCountsForQuestion(context, questionId);
                        
                        // set the question for each answer
                        question.Answers.ForEach(
                            x =>
                                {
                                    x.UserVotes = voteCounts.ContainsKey(x.AnswerId)
                                                      ? voteCounts[x.AnswerId]
                                                      : new List<int>();

                                    x.Question = question;
                                });
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ConverterFactory.ConvertOne<Question, T>(question);
        }

        private static IDictionary<int, List<int>> GetVoteCountsForQuestion(DilemmaContext context, int questionId)
        {
            return context.Vote.Where(x => x.Question.QuestionId == questionId).Select(
                x => new
                         {
                             x.Answer.AnswerId,
                             x.User.UserId
                         }).GroupBy(x => x.AnswerId).ToDictionary(g => g.Key, g => g.Select(x => x.UserId).ToList());
        }

        private static bool HasUserAnsweredQuestion(DilemmaContext context, int userId, int questionId)
        {
            return
                context.Answers.AsNoTracking()
                    .Where(x => x.Question.QuestionId == questionId)
                    .Where(x => x.User.UserId == userId)
                    .Any(x => x.AnswerState != AnswerState.ReservedSlot);
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
