using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="ModerationViewModel"/>.
    /// </summary>
    public static class ModerationViewModelConverter
    {
        /// <summary>
        /// Converts an <see cref="Moderation"/> to an <see cref="ModerationViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Moderation"/> to convert.</param>
        /// <returns>The resultant <see cref="ModerationViewModel"/>.</returns>
        public static ModerationViewModel FromModeration(Moderation model)
        {
            return new ModerationViewModel
                {
                    ModerationId = model.ModerationId,
                    ModerationEntries = ConverterFactory.ConvertMany<ModerationEntry, ModerationEntryViewModel>(model.ModerationEntries).ToList()
                };
        }
    }
}
