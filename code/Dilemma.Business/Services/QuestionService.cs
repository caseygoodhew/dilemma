using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Question service provider.
    /// </summary>
    internal class QuestionService : IQuestionService
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();

        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();

        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        private static readonly Lazy<IQuestionRepository> QuestionRepository = Locator.Lazy<IQuestionRepository>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        private static readonly Lazy<INotificationService> NotificationService = Locator.Lazy<INotificationService>();

        /// <summary>
        /// Saves a new <see cref="QuestionViewModel"/> instance.
        /// </summary>
        /// <param name="questionViewModel">The <see cref="QuestionViewModel"/> to save.</param>
        /// <returns>The new question id.</returns>
        public int SaveNewQuestion(QuestionViewModel questionViewModel)
        {
            var systemConfiguration = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();

            SetMaxAnswers(systemConfiguration, questionViewModel);
            SetTimeframes(systemConfiguration, questionViewModel);

            questionViewModel.Text = questionViewModel.Text.TidyWhiteSpace();

            var userId = SecurityManager.Value.GetUserId();
            
            return QuestionRepository.Value.CreateQuestion(userId, questionViewModel);
        }

        /// <summary>
        /// Gets all questions as <see cref="QuestionViewModel"/>s.
        /// </summary>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        public IEnumerable<QuestionViewModel> GetAllQuestions()
        {
            return MarkBookmarks(QuestionRepository.Value.QuestionList<QuestionViewModel>(null));
        }

        public IEnumerable<QuestionViewModel> GetQuestions(CategoryViewModel category)
        {
            // TODO: This is incorrect!!!
            return GetAllQuestions();
        }

        /// <summary>
        /// Gets all questions that the current user has been involved in
        /// </summary>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        public IEnumerable<QuestionViewModel> GetMyActivity()
        {
            return MarkBookmarks(QuestionRepository.Value.QuestionList<QuestionViewModel>(SecurityManager.Value.GetUserId()));
        }

        /// <summary>
        /// Gets a single <see cref="QuestionDetailsViewModel"/> by id.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <returns>The <see cref="QuestionDetailsViewModel"/>.</returns>
        public QuestionDetailsViewModel GetQuestion(int questionId)
        {
            var question = QuestionRepository.Value.GetQuestion<QuestionDetailsViewModel>(SecurityManager.Value.GetUserId(), questionId, GetQuestionAs.FullDetails);

            if (question != null)
            {
                NotificationService.Value.Mute(NotificationLookupBy.QuestionId, questionId);
            }
            
            return MarkBookmarks(question);
        }

        /// <summary>
        /// Requests an an slot for the given question id. If no slot is available, null is returned.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <returns>The answer id if a slot is available or null if it is not available.</returns>
        public int? RequestAnswerSlot(int questionId)
        {
            var userId = SecurityManager.Value.GetUserId();
            return QuestionRepository.Value.RequestAnswerSlot(userId, questionId);
        }

        /// <summary>
        /// Gets an answer in progress by question id and answer id. The provided answer id must be an answer of the question id.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerId">The answer id.</param>
        /// <returns>The <see cref="AnswerViewModel"/>.</returns>
        public AnswerViewModel GetAnswerInProgress(int questionId, int answerId)
        {
            var userId = SecurityManager.Value.GetUserId(); 
            return QuestionRepository.Value.GetAnswerInProgress<AnswerViewModel>(userId, questionId, answerId);
        }

        /// <summary>
        /// Touches an answer so that the answer slot does not expire.
        /// </summary>
        /// <param name="answerId">The id of the answer to touch.</param>
        public void TouchAnswer(int answerId)
        {
            var userId = SecurityManager.Value.GetUserId();
            QuestionRepository.Value.TouchAnswer(userId, answerId);
        }

        /// <summary>
        /// Completes an answer that is in an initial 'Answer slot' state.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerViewModel">The <see cref="AnswerViewModel"/> to save.</param>
        /// <returns>Flag indicating if the answer was completed.</returns>
        public bool CompleteAnswer(int questionId, AnswerViewModel answerViewModel)
        {
            var userId = SecurityManager.Value.GetUserId(); 
            answerViewModel.Text = answerViewModel.Text.TidyWhiteSpace();
            return QuestionRepository.Value.CompleteAnswer(userId, questionId, answerViewModel);
        }

        /// <summary>
        /// Registers a users vote for an answer.
        /// </summary>
        /// <param name="answerId">The answer to register the vote against.</param>
        public void RegisterVote(int answerId)
        {
            var userId = SecurityManager.Value.GetUserId();
            QuestionRepository.Value.RegisterVote(userId, answerId);
        }

        /// <summary>
        /// Deregisters a users vote for an answer.
        /// </summary>
        /// <param name="answerId">The answer to deregister the vote against.</param>
        public void DeregisterVote(int answerId)
        {
            var userId = SecurityManager.Value.GetUserId();
            var questionOwnerUserId = QuestionRepository.Value.GetQuestionUserIdByAnswerId(answerId);

            // cannot remove star vote
            if (userId == questionOwnerUserId)
            {
                return;
            }

            QuestionRepository.Value.DeregisterVote(userId, answerId);
        }

        public void AddBookmark(int questionId)
        {
            QuestionRepository.Value.AddBookmark(SecurityManager.Value.GetUserId(), questionId);
        }

        public void RemoveBookmark(int questionId)
        {
            QuestionRepository.Value.RemoveBookmark(SecurityManager.Value.GetUserId(), questionId);
        }

        private static void SetMaxAnswers(SystemConfiguration systemConfiguration, QuestionViewModel questionViewModel)
        {
            if (systemConfiguration.SystemEnvironment == SystemEnvironment.Production || !questionViewModel.MaxAnswers.HasValue)
            {
                questionViewModel.MaxAnswers = systemConfiguration.MaxAnswers;
            }
        }

        private static void SetTimeframes(SystemConfiguration systemConfiguration, QuestionViewModel questionViewModel)
        {
            var now = TimeSource.Value.Now;
            questionViewModel.CreatedDateTime = now;

            switch (systemConfiguration.QuestionLifetime)
            {
                case QuestionLifetime.OneMinute:
                    questionViewModel.ClosesDateTime = now.AddMinutes(1);
                    break;
                case QuestionLifetime.FiveMinutes:
                    questionViewModel.ClosesDateTime = now.AddMinutes(5);
                    break;
                case QuestionLifetime.OneDay:
                    questionViewModel.ClosesDateTime = now.AddDays(1);
                    break;
                case QuestionLifetime.OneYear:
                    questionViewModel.ClosesDateTime = now.AddYears(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static QuestionDetailsViewModel MarkBookmarks(QuestionDetailsViewModel question)
        {
            MarkBookmarks(question.QuestionViewModel);
            return question;
        }

        private static QuestionViewModel MarkBookmarks(QuestionViewModel question)
        {
            return MarkBookmarks(new [] { question }).Single();
        }

        private static IEnumerable<QuestionViewModel> MarkBookmarks(IEnumerable<QuestionViewModel> questions)
        {
            var bookmarks = QuestionRepository.Value.GetBookmarkedQuestionIds(SecurityManager.Value.GetUserId()).ToList();
            var questionList = questions.ToList();

            foreach (var question in questionList.Where(x => x != null && x.QuestionId.HasValue))
            {
                question.IsBookmarked = bookmarks.Contains(question.QuestionId.Value);
            }

            return questionList;
        }
    }
}
