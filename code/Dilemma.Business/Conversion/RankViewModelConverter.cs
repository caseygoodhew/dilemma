using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="RankViewModel"/>.
    /// </summary>
    public static class RankViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="Rank"/> to a <see cref="RankViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Rank"/> to convert.</param>
        /// <returns>The resultant <see cref="RankViewModel"/>.</returns>
        public static RankViewModel FromRank(Rank model)
        {
            return new RankViewModel
                       {
                           Id = model.Id,
                           Level = model.Level,
                           Name = model.Name,
                           PointsRequired = model.PointsRequired
                       };
        }
    }
}