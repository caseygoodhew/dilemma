using System;
using System.Collections.Generic;

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
        /// Initializes or reinitializes a <see cref="CreateQuestionViewModel"/>. Reinitialization allows a view model to return the correct state on POST validation.
        /// </summary>
        /// <param name="questionViewModel">(Optional) The <see cref="CreateQuestionViewModel"/> to reinitialize.</param>
        /// <returns>The <see cref="CreateQuestionViewModel"/>.</returns>
        public CreateQuestionViewModel InitNewQuestion(CreateQuestionViewModel questionViewModel = null)
        {
            var systemConfiguration = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();
            
            if (questionViewModel == null)
            {
                questionViewModel = new CreateQuestionViewModel();
                
                if (systemConfiguration.IsInternalEnvironment)
                {
                    questionViewModel.Text = "\n\n\n\nThis text makes it easier to reach the minimum 50 character limit. This text will not show in a production environment.";
                }
            }

            questionViewModel.Categories = SiteService.Value.GetCategories();
            questionViewModel.MaxAnswers = systemConfiguration.MaxAnswers;
            questionViewModel.QuestionLifetime = systemConfiguration.QuestionLifetime;
            questionViewModel.ShowTestingOptions = systemConfiguration.IsInternalEnvironment;

            return questionViewModel;
        }

        /// <summary>
        /// Saves a new <see cref="CreateQuestionViewModel"/> instance.
        /// </summary>
        /// <param name="questionViewModel">The <see cref="CreateQuestionViewModel"/> to save.</param>
        /// <returns>The new question id.</returns>
        public int SaveNewQuestion(CreateQuestionViewModel questionViewModel)
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
            return QuestionRepository.Value.QuestionList<QuestionViewModel>(null);
        }

        /// <summary>
        /// Gets all questions that the current user has been involved in
        /// </summary>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        public IEnumerable<QuestionViewModel> GetMyActivity()
        {
            return QuestionRepository.Value.QuestionList<QuestionViewModel>(SecurityManager.Value.GetUserId());
        }

        /// <summary>
        /// Gets a single <see cref="QuestionDetailsViewModel"/> by id.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <returns>The <see cref="QuestionDetailsViewModel"/>.</returns>
        public QuestionDetailsViewModel GetQuestion(int questionId)
        {
            var question = QuestionRepository.Value.GetQuestion<QuestionDetailsViewModel>(questionId, GetQuestionAs.FullDetails);
            
            if (question != null)
            {
                NotificationService.Value.Mute(NotificationLookupBy.QuestionId, questionId);
            }
            
            return question;
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

        private void SetMaxAnswers(SystemConfiguration systemConfiguration, QuestionViewModel questionViewModel)
        {
            if (systemConfiguration.SystemEnvironment == SystemEnvironment.Production || !questionViewModel.MaxAnswers.HasValue)
            {
                questionViewModel.MaxAnswers = systemConfiguration.MaxAnswers;
            }
        }

        private void SetTimeframes(SystemConfiguration systemConfiguration, CreateQuestionViewModel questionViewModel)
        {
            var now = TimeSource.Value.Now;
            questionViewModel.CreatedDateTime = now;

            switch (GetQuestionLifetime(systemConfiguration, questionViewModel))
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

        private QuestionLifetime GetQuestionLifetime(SystemConfiguration systemConfiguration, CreateQuestionViewModel questionViewModel)
        {
            return systemConfiguration.IsInternalEnvironment ? questionViewModel.QuestionLifetime : systemConfiguration.QuestionLifetime;
        }
    }
}
