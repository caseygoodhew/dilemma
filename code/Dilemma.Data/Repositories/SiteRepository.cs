using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Caching;
using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Site repository services implementation.
    /// </summary>
    internal class SiteRepository : ISiteRepository
    {
        private static readonly Lazy<IProviderCache> Cache = Locator.Lazy<IProviderCache>();

        /// <summary>
        /// Gets the <see cref="Category"/> list in the specified type. There must be a converter registered between <see cref="Category"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>A list of <see cref="Category"/>s converted to type T.</returns>
        public IList<T> GetCategories<T>() where T : class
        {
            var categories = Cache.Value.Get<IList<Category>>(
                () =>
                    {
                        using (var context = new DilemmaContext())
                        {
                            return context.Categories.ToList();
                        }
                    });
            
            return ConverterFactory.ConvertMany<Category, T>(categories).ToList();
        }

        public IList<T> GetRanks<T>() where T : class
        {
            var ranks = GetRanks();
            return ConverterFactory.ConvertMany<Rank, T>(ranks).ToList();
        }

        public T GetRankByPoints<T>(int points) where T : class
        {
            var ranks = GetRanks().OrderByDescending(x => x.PointsRequired);
            var rank = ranks.SkipWhile(x => x.PointsRequired > points).FirstOrDefault() ?? ranks.Last();

            return ConverterFactory.ConvertOne<Rank, T>(rank);
        }

        private static IEnumerable<Rank> GetRanks()
        {
            return Cache.Value.Get<IList<Rank>>(
                () =>
                {
                    using (var context = new DilemmaContext())
                    {
                        return context.Ranks.ToList().OrderBy(x => x.PointsRequired).Select(
                            (x, i) =>
                            {
                                x.Level = i + 1;
                                return x;
                            }).ToList();
                    }
                });
        }
    }
}
