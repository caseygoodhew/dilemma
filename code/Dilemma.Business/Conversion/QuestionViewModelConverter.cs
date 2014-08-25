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
    public static class QuestionViewModelConverter
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);
        
        public static Question ToQuestion(QuestionViewModel viewModel)
        {
            return new Question
                       {
                           Text = viewModel.Text.Trim(),
                           CreatedDateTime = viewModel.CreatedDateTime.GuardedValue("CreatedDateTime"),
                           ClosesDateTime = viewModel.ClosesDateTime.GuardedValue("ClosesDateTime"),
                           Category = new Category { CategoryId = viewModel.CategoryId.GuardedValue("CategoryId") },
                           MaxAnswers = viewModel.MaxAnswers.GuardedValue("MaxAnswers")
                       };
        }

        public static QuestionViewModel FromQuestion(Question model)
        {
            var answers = ConverterFactory.ConvertMany<Answer, AnswerViewModel>(model.Answers);
            
            return new QuestionViewModel
                {
                    QuestionId = model.QuestionId,
                    Text = model.Text,
                    CreatedDateTime = model.CreatedDateTime,
                    ClosesDateTime = model.ClosesDateTime,
                    CategoryId = model.Category.CategoryId,
                    CategoryName = model.Category.Name,
                    TotalAnswers = model.TotalAnswers,
                    MaxAnswers = model.MaxAnswers,
                    Answers = (answers ?? Enumerable.Empty<AnswerViewModel>()).ToList()
                };
        }

        
    }
}
