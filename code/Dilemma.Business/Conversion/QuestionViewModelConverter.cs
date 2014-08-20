using System;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Extensions;

namespace Dilemma.Business.Conversion
{
    public static class QuestionViewModelConverter
    {
        public static Question ToQuestion(QuestionViewModel viewModel)
        {
            Guard.ArgumentNotNull(viewModel.CategoryId, "QuestionViewModel.CategoryId");
            
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
            return new QuestionViewModel
                {
                    QuestionId = model.QuestionId,
                    Text = model.Text,
                    CreatedDateTime = model.CreatedDateTime,
                    ClosesDateTime = model.ClosesDateTime,
                    CategoryId = model.Category.CategoryId,
                    CategoryName = model.Category.Name,
                    AnswerCount = 1,
                    MaxAnswers = model.MaxAnswers
                };
        }
    }
}
