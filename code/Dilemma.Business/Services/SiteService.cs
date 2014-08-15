using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class SiteService : ISiteService
    {
        private static readonly Lazy<ISiteRepository> SiteRepository =
            new Lazy<ISiteRepository>(Locator.Current.Instance<ISiteRepository>);
        
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            return SiteRepository.Value.GetCategories<CategoryViewModel>();
        }
    }
}