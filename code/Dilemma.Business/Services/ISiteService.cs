using System.Collections;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Site service provider interface.
    /// </summary>
    public interface ISiteService
    {
        /// <summary>
        /// Gets a list of <see cref="CategoryViewModel"/>s.
        /// </summary>
        /// <returns>The <see cref="CategoryViewModel"/>s.</returns>
        IEnumerable<CategoryViewModel> GetCategories();

        RankViewModel GetRankByPoints(int points);

        IEnumerable<RankViewModel> GetRanks();
    }
}
