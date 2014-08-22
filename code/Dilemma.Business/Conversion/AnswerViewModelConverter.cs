using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common;
using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Extensions;

namespace Dilemma.Business.Conversion
{
    public static class AnswerViewModelConverter
    {
        public static Answer ToAnswer(AnswerViewModel viewModel)
        {
            return new Answer
                       {
                           Text = viewModel.Text.Trim(),
                           CreatedDateTime = viewModel.CreatedDateTime.GuardedValue("CreatedDateTime"),
                       };
        }

        public static AnswerViewModel FromAnswer(Answer model)
        {
            return new AnswerViewModel
                {
                    AnswerId = model.AnswerId,
                    Text = model.Text,
                    CreatedDateTime = model.CreatedDateTime
                };
        }
    }
}
