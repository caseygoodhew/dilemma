using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Site service provider.
    /// </summary>
    internal class SiteService : ISiteService
    {
        private static readonly Lazy<ISiteRepository> SiteRepository =
            new Lazy<ISiteRepository>(Locator.Current.Instance<ISiteRepository>);

        /// <summary>
        /// Gets a list of <see cref="CategoryViewModel"/>s.
        /// </summary>
        /// <returns>The <see cref="CategoryViewModel"/>s.</returns>
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            return SiteRepository.Value.GetCategories<CategoryViewModel>();
        }
    }
}