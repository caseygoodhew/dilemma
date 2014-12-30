using System;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    public static class Answers
    {
        private static readonly Lazy<IQuestionService> _questionService = Locator.Lazy<IQuestionService>();

        public static int? RequestAnswerSlot(int questionId, string answerReference)
        {
            return ObjectDictionary.Get(
                ObjectType.Answer,
                answerReference,
                () => _questionService.Value.RequestAnswerSlot(questionId));
        }

        public static int? RequestAnswerSlot(string questionReference, string answerReference)
        {
            return RequestAnswerSlot(
                ObjectDictionary.Get<int>(ObjectType.Question, questionReference),
                answerReference);
        }

        public static AnswerViewModel GetAnswerInProgress(int questionId, int answerId)
        {
            return _questionService.Value.GetAnswerInProgress(questionId, answerId);
        }

        public static AnswerViewModel GetAnswerInProgress(string questionReference, string answerReference)
        {
            return GetAnswerInProgress(
                ObjectDictionary.Get<int>(ObjectType.Question, questionReference),
                ObjectDictionary.Get<int>(ObjectType.Answer, answerReference));
        }

        public static bool CompleteAnswer(int questionId, AnswerViewModel answerViewModel)
        {
            return _questionService.Value.CompleteAnswer(questionId, answerViewModel);
        }

        public static bool CompleteAnswer(string questionReference, AnswerViewModel answerViewModel)
        {
            return CompleteAnswer(
                ObjectDictionary.Get<int>(ObjectType.Question, questionReference),
                answerViewModel);
        }

        public static AnswerViewModel FillDefaults(string reference)
        {
            return FillDefaults(new AnswerViewModel
                                    {
                                        AnswerId = ObjectDictionary.Get<int>(ObjectType.Answer, reference)
                                    });
        }
        
        public static T FillDefaults<T>(T viewModel) where T : AnswerViewModel
        {
            viewModel.Text = "Integration testing answer";

            return viewModel;
        }
    }
}