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
    internal class QuestionService : IQuestionService
    {
        private static readonly Lazy<IAdministrationService> AdministrationService = new Lazy<IAdministrationService>(Locator.Current.Instance<IAdministrationService>);

        private static readonly Lazy<ISiteService> SiteService = new Lazy<ISiteService>(Locator.Current.Instance<ISiteService>);
        
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);
        
        private static readonly Lazy<IQuestionRepository> QuestionRepository = new Lazy<IQuestionRepository>(Locator.Current.Instance<IQuestionRepository>);

        public CreateQuestionViewModel InitNew(CreateQuestionViewModel questionViewModel = null)
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

        public void SaveNew(CreateQuestionViewModel questionViewModel)
        {
            var systemConfiguration = AdministrationService.Value.GetSystemConfiguration();

            SetMaxAnswers(systemConfiguration, questionViewModel);
            SetTimeframes(systemConfiguration, questionViewModel);

            questionViewModel.Text = questionViewModel.Text.TidyWhiteSpace();

            QuestionRepository.Value.Create(questionViewModel);
        }

        public IEnumerable<QuestionViewModel> GetAll()
        {
            return QuestionRepository.Value.List<QuestionViewModel>();
        }

        public QuestionDetailsViewModel GetQuestion(int questionId)
        {
            return QuestionRepository.Value.Get<QuestionDetailsViewModel>(questionId, GetQuestionAs.FullDetails);
        }

        public int? RequestAnswerSlot(int questionId)
        {
            return QuestionRepository.Value.RequestAnswerSlot(questionId);
        }

        public AnswerViewModel GetAnswerInProgress(int questionId, int answerId)
        {
            return QuestionRepository.Value.GetAnswerInProgress<AnswerViewModel>(questionId, answerId);
        }

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
