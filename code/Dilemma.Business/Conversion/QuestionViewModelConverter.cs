using System;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.ServiceLocator;
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
                           Text = viewModel.Text,
                           CreatedDateTime = viewModel.CreatedDateTime ?? TimeSource.Value.Now
                       };
        }

        public static QuestionViewModel FromQuestion(Question model)
        {
            return new QuestionViewModel
                       {
                           Text = model.Text,
                           CreatedDateTime = model.CreatedDateTime
                       };
        }
    }
}
