using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common;
using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Extensions;
using Disposable.Common.Services;

namespace Dilemma.Business.Conversion
{
    public static class QuestionDetailsViewModelConverter
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);

        public static Question ToQuestion(QuestionDetailsViewModel viewModel)
        {
            var question = ConverterFactory.ConvertOne<QuestionViewModel, Question>(viewModel.QuestionViewModel);

            if (viewModel.Answer != null && viewModel.Answer.AnswerId != null)
            {
                question.Answers.Add(ConverterFactory.ConvertOne<AnswerViewModel, Answer>(viewModel.Answer));
            }

            return question;
        }

        public static QuestionDetailsViewModel FromQuestion(Question model)
        {
            return new QuestionDetailsViewModel
                       {
                           QuestionViewModel = ConverterFactory.ConvertOne<Question, QuestionViewModel>(model),
                           CanAnswer = CanAnswer(model)
                       };
        }

        private static bool CanAnswer(Question model)
        {
            var now = TimeSource.Value.Now;

            return model.AnswerCount < model.MaxAnswers && model.ClosesDateTime > now;
        }
    }
}
