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
                        var context = new DilemmaContext();
                        return context.Categories.ToList();
                    });
            
            return ConverterFactory.ConvertMany<Category, T>(categories).ToList();
        }
    }
}
