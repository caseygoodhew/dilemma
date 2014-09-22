using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Question service provider implementation.
    /// </summary>
    internal class QuestionRepository : IQuestionRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);

        private static readonly Lazy<INotificationRepository> NotificationRepository = new Lazy<INotificationRepository>(Locator.Current.Instance<INotificationRepository>);

        /// <summary>
        /// Creates a <see cref="Question"/> from the specified type. There must be a converter registered between <see cref="T"/> and <see cref="Question"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user creating the question.</param>
        /// <param name="questionType">The convertable instance</param>
        public void CreateQuestion<T>(int userId, T questionType) where T : class
        {
            var question = ConverterFactory.ConvertOne<T, Question>(questionType);

            using (var context = new DilemmaContext())
            {
                question.User = new User { UserId = userId };

                context.Users.Attach(question.User);
                context.Categories.Attach(question.Category);
                context.Questions.Add(question);
                context.SaveChangesVerbose();
            }
        }

        /// <summary>
        /// Gets the <see cref="Question"/> in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="questionId">The id of the question to get.</param>
        /// <param name="config">Options for what extended data to include.</param>
        /// <returns>The <see cref="Question"/> converted to type T.</returns>
        public T GetQuestion<T>(int questionId, GetQuestionAs config) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return GetQuestion<T>(context, questionId, config);
            }
        }

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="userId">The id of user to get the activity for. If this is null, all quesitons will be returned.</param>
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
                                    TotalAnswer = x.Answers.Count,
                                    x.MaxAnswers,
                                    x.Category,
                                    x.Text,
                                    x.ClosesDateTime,
                                    x.CreatedDateTime,
                                    x.User.UserId,
                                    MostRecentActivity = x.Answers.Where(a => a.AnswerType == AnswerType.Completed).Select(a => a.CreatedDateTime).Concat(new [] { x.CreatedDateTime }).Max()
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
                
                var question = GetQuestion<Question>(context, questionId, GetQuestionAs.AnswerCount);

                if (question.TotalAnswers >= question.MaxAnswers || question.ClosesDateTime < TimeSource.Value.Now)
                {
                    return null;
                }

                var answer = new Answer
                                 {
                                     CreatedDateTime = TimeSource.Value.Now,
                                     Question = question,
                                     User = new User { UserId = userId }
                                 };
                
                context.Questions.Attach(question);
                context.Users.Attach(answer.User);
                
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
            
                answer.CreatedDateTime = TimeSource.Value.Now;
                answer.AnswerType = AnswerType.Completed;

                context.Answers.Update(context, answer);
                context.SaveChangesVerbose();

                NotificationRepository.Value.Raise(existingAnswer.Question.User.UserId, NotificationType.QuestionAnswered, answer.AnswerId);

                return true;
            }
        }

        private static T GetQuestion<T>(DilemmaContext context, int questionId, GetQuestionAs config) where T : class
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
                                                 TotalAnswers = x.Answers.Count
                                             })
                            .AsEnumerable()
                            .Select(x => new Question
                                             {
                                                 QuestionId = x.QuestionId,
                                                 MaxAnswers = x.MaxAnswers,
                                                 ClosesDateTime = x.ClosesDateTime,
                                                 User = new User { UserId = x.UserId },
                                                 TotalAnswers = x.TotalAnswers
                                             })
                            .Single();

                    break;

                case GetQuestionAs.FullDetails:
                    question =
                        context.Questions.Where(x => x.QuestionId == questionId)
                            .Include(x => x.User)
                            .Include(x => x.Category)
                            .Include(x => x.Answers)
                            .Include(x => x.Answers.Select(a => a.User))
                            .Single();
                    
                    question.TotalAnswers = question.Answers.Count;
                    question.Answers = question.Answers.Where(x => x.AnswerType == AnswerType.Completed).ToList();
                    
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
                    .Where(x => x.AnswerType == AnswerType.ReservedSlot);

            if (answerId.HasValue)
            {
                query = query.Where(x => x.AnswerId == answerId);
            }

            return query.FirstOrDefault();
        }
    }
}
