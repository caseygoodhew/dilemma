using System.Collections.Generic;

namespace Dilemma.Data.Repositories
{
    public interface ISiteRepository
    {
        IList<T> GetCategories<T>() where T : class;
    }
}
