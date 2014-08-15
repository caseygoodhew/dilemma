using System;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Business.Conversion
{
    public static class QuestionViewModelConverter
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);
        
        public static Question ToQuestion(QuestionViewModel viewModel)
        {
            Guard.ArgumentNotNull(viewModel.CategoryId, "QuestionViewModel.CategoryId");
            
            return new Question
                       {
                           Text = viewModel.Text.Trim(),
                           CreatedDateTime = viewModel.CreatedDateTime ?? TimeSource.Value.Now,
                           Category = new Category { CategoryId = viewModel.CategoryId.Value }
                       };
        }

        public static QuestionViewModel FromQuestion(Question model)
        {
            return new QuestionViewModel
                {
                    QuestionId = model.QuestionId,
                    Text = model.Text,
                    CreatedDateTime = model.CreatedDateTime,
                    CategoryId = model.Category.CategoryId,
                    CategoryName = model.Category.Name
                };
        }
    }
}
