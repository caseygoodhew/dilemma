using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    public static class Questions
    {
        private static readonly Lazy<IQuestionService> _questionService = Locator.Lazy<IQuestionService>();

        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();

        private static readonly Random Random = new Random();

        public static int SaveNewQuestion(string reference, QuestionViewModel questionViewModel)
        {
            return ObjectDictionary.Get(
                ObjectType.Question,
                reference,
                () => _questionService.Value.SaveNewQuestion(questionViewModel));
        }

        public static int CreateNewQuestion(string reference)
        {
            return SaveNewQuestion(reference, FillDefaults(new QuestionViewModel()));
        }

        public static int CreateAndApproveQuestion(string reference)
        {
            var questionId = SaveNewQuestion(reference, FillDefaults(new QuestionViewModel()));
            var moderation = ManualModeration.GetNextForUser(SecurityManager.GetUserId());
            ManualModeration.Approve(moderation.ModerationId);
            return questionId;
        }

        public static IEnumerable<QuestionViewModel> GetAllQuestions()
        {
            return _questionService.Value.GetAllQuestions();
        }

        public static IEnumerable<QuestionViewModel> GetMyActivity()
        {
            return _questionService.Value.GetMyActivity();
        }

        public static QuestionDetailsViewModel GetQuestion(int questionId)
        {
            return _questionService.Value.GetQuestion(questionId);
        }

        public static QuestionDetailsViewModel GetQuestion(string reference)
        {
            return GetQuestion
                (ObjectDictionary.Get<int>(ObjectType.Question, 
                reference));
        }

        public static T FillDefaults<T>(T viewModel) where T : QuestionViewModel
        {
            var categories = SiteService.Value.GetCategories().ToList();

            viewModel.CategoryId = categories.Skip(Random.Next(0, categories.Count - 1)).First().CategoryId;
            viewModel.Text = "Integration testing question";

            return viewModel;
        }
    }
}
