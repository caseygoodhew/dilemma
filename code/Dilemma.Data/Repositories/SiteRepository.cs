using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;

namespace Dilemma.Data.Repositories
{
    internal class SiteRepository : ISiteRepository
    {
        public IList<T> GetCategories<T>() where T : class
        {
            var context = new DilemmaContext();
            return ConverterFactory.ConvertMany<Category, T>(context.Categories).ToList();
        }
    }
}
