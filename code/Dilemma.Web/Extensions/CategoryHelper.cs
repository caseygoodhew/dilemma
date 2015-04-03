using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Extensions
{
    public static class CategoryHelper
    {
        public static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();
        
        private static readonly IList<string> VirtualCategories = new[] { "bookmarks" };
        
        public static string ValidateCategory(string category, bool excludeVirtualCategories = false)
        {
            if (!excludeVirtualCategories && VirtualCategories.Contains(category, StringComparer.CurrentCultureIgnoreCase))
            {
                return category;
            }

            return GetCategory(category) == null ? string.Empty : category;
        }

        public static CategoryViewModel GetCategory(string category)
        {
            return
                SiteService.Value.GetCategories()
                    .FirstOrDefault(x => x.Name.Equals(category, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}