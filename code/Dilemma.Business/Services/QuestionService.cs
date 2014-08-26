using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

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
        private static readonly Lazy<IAdministrationService> AdministrationService = new Lazy<IAdministrationService>(Locator.Current.Instance<IAdministrationService>);

        private static readonly Lazy<ISiteService> SiteService = new Lazy<ISiteService>(Locator.Current.Instance<ISiteService>);
        
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);
        
        private static readonly Lazy<IQuestionRepository> QuestionRepository = new Lazy<IQuestionRepository>(Locator.Current.Instance<IQuestionRepository>);

        /// <summary>
        /// Initializes or reinitializes a <see cref="CreateQuestionViewModel"/>. Reinitialization allows a view model to return the correct state on POST validation.
        /// </summary>
        /// <param name="questionViewModel">(Optional) The <see cref="CreateQuestionViewModel"/> to reinitialize.</param>
        /// <returns>The <see cref="CreateQuestionViewModel"/>.</returns>
        public CreateQuestionViewModel InitNewQuestion(CreateQuestionViewModel questionViewModel = null)
        {
            var systemConfiguration = AdministrationService.Value.GetSystemConfiguration();
            var allowTestingConfiguration = AllowTestingConfiguration(systemConfiguration);

            if (questionViewModel == null)
            {
                questionViewModel = new CreateQuestionViewModel();
                if (allowTestingConfiguration)
                {
                    questionViewModel.Text = "\n\n\n\nThis text makes it easier to reach the minimum 50 character limit. This text will not show in a production environment.";
                }
            }

            questionViewModel.Categories = SiteService.Value.GetCategories();
            questionViewModel.MaxAnswers = systemConfiguration.MaxAnswers;
            questionViewModel.QuestionLifetime = systemConfiguration.QuestionLifetime;
            questionViewModel.ShowTestingOptions = allowTestingConfiguration;

            return questionViewModel;
        }

        /// <summary>
        /// Saves a new <see cref="CreateQuestionViewModel"/> instance.
        /// </summary>
        /// <param name="questionViewModel">The <see cref="CreateQuestionViewModel"/> to save.</param>
        public void SaveNewQuestion(CreateQuestionViewModel questionViewModel)
        {
            var systemConfiguration = AdministrationService.Value.GetSystemConfiguration();

            SetMaxAnswers(systemConfiguration, questionViewModel);
            SetTimeframes(systemConfiguration, questionViewModel);

            questionViewModel.Text = questionViewModel.Text.TidyWhiteSpace();

            QuestionRepository.Value.CreateQuestion(questionViewModel);
        }

        /// <summary>
        /// Gets all questions as <see cref="QuestionViewModel"/>s.
        /// </summary>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        public IEnumerable<QuestionViewModel> GetAllQuestions()
        {
            return QuestionRepository.Value.QuestionList<QuestionViewModel>();
        }

        /// <summary>
        /// Gets a single <see cref="QuestionDetailsViewModel"/> by id.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <returns>The <see cref="QuestionDetailsViewModel"/>.</returns>
        public QuestionDetailsViewModel GetQuestion(int questionId)
        {
            return QuestionRepository.Value.GetQuestion<QuestionDetailsViewModel>(questionId, GetQuestionAs.FullDetails);
        }

        /// <summary>
        /// Requests an an slot for the given question id. If no slot is available, null is returned.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <returns>The answer id if a slot is available or null if it is not available.</returns>
        public int? RequestAnswerSlot(int questionId)
        {
            return QuestionRepository.Value.RequestAnswerSlot(questionId);
        }

        /// <summary>
        /// Gets an answer in progress by question id and answer id. The provided answer id must be an answer of the question id.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerId">The answer id.</param>
        /// <returns>The <see cref="AnswerViewModel"/>.</returns>
        public AnswerViewModel GetAnswerInProgress(int questionId, int answerId)
        {
            return QuestionRepository.Value.GetAnswerInProgress<AnswerViewModel>(questionId, answerId);
        }

        /// <summary>
        /// Completes an answer that is in an initial 'Answer slot' state.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerViewModel">The <see cref="AnswerViewModel"/> to save.</param>
        public void CompleteAnswer(int questionId, AnswerViewModel answerViewModel)
        {
            answerViewModel.Text = answerViewModel.Text.TidyWhiteSpace();
            QuestionRepository.Value.CompleteAnswer(questionId, answerViewModel);
        }

        private void SetMaxAnswers(SystemConfigurationViewModel systemConfiguration, QuestionViewModel questionViewModel)
        {
            if (systemConfiguration.SystemEnvironment == SystemEnvironment.Production || !questionViewModel.MaxAnswers.HasValue)
            {
                questionViewModel.MaxAnswers = systemConfiguration.MaxAnswers;
            }
        }

        private void SetTimeframes(SystemConfigurationViewModel systemConfiguration, CreateQuestionViewModel questionViewModel)
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

        private QuestionLifetime GetQuestionLifetime(SystemConfigurationViewModel systemConfiguration, CreateQuestionViewModel questionViewModel)
        {
            return AllowTestingConfiguration(systemConfiguration) ? questionViewModel.QuestionLifetime : systemConfiguration.QuestionLifetime;
        }

        private bool AllowTestingConfiguration(SystemConfigurationViewModel systemConfiguration)
        {
            return systemConfiguration.SystemEnvironment == SystemEnvironment.Development
                   || systemConfiguration.SystemEnvironment == SystemEnvironment.Testing;
        }
    }
}
