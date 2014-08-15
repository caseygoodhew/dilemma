using System.Collections;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    public interface ISiteService
    {
        IEnumerable<CategoryViewModel> GetCategories();
    }
}
