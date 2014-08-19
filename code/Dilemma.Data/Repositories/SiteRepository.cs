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
    internal class SiteRepository : ISiteRepository
    {
        private static readonly Lazy<IProviderCache> Cache = new Lazy<IProviderCache>(Locator.Current.Instance<IProviderCache>);
        
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
