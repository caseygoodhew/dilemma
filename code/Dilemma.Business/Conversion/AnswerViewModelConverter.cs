using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.Extensions;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="AnswerViewModel"/>.
    /// </summary>
    public static class AnswerViewModelConverter
    {
        /// <summary>
        /// Converts an <see cref="AnswerViewModel"/> to an <see cref="Answer"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="AnswerViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="Answer"/>.</returns>
        public static Answer ToAnswer(AnswerViewModel viewModel)
        {
            return new Answer
                       {
                           AnswerId = viewModel.AnswerId.GuardedValue(),
                           Text = viewModel.Text.Trim(),
                           CreatedDateTime = viewModel.CreatedDateTime
                       };
        }

        /// <summary>
        /// Converts an <see cref="Answer"/> to an <see cref="AnswerViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Answer"/> to convert.</param>
        /// <returns>The resultant <see cref="AnswerViewModel"/>.</returns>
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
