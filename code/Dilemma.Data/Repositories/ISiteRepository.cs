using System.Collections.Generic;

using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Site repository services interface.
    /// </summary>
    public interface ISiteRepository
    {
        /// <summary>
        /// Gets the <see cref="Category"/> list in the specified type. There must be a converter registered between <see cref="Category"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>A list of <see cref="Category"/>s converted to type T.</returns>
        IList<T> GetCategories<T>() where T : class;

        IList<T> GetRanks<T>() where T : class;

        T GetRankByPoints<T>(int points) where T : class;
    }
}
