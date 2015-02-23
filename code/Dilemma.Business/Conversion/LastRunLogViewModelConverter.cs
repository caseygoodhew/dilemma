using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="LastRunLogViewModel"/>.
    /// </summary>
    public static class LastRunLogViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="LastRunLog"/> to a <see cref="LastRunLogViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="LastRunLog"/> to convert.</param>
        /// <returns>The resultant <see cref="LastRunLogViewModel"/>.</returns>
        public static LastRunLogViewModel FromLastRunLog(LastRunLog model)
        {
            return new LastRunLogViewModel
                       {
                           CloseQuestions = model.CloseQuestions,
                           ExpireAnswerSlots = model.ExpireAnswerSlots,
                           RetireOldQuestions = model.RetireOldQuestions
                       };
        }
    }
}