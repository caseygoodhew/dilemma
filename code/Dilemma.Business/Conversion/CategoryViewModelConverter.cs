using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    public static class CategoryViewModelConverter
    {
        public static Category ToCategory(CategoryViewModel viewModel)
        {
            return new Category
            {
                CategoryId = viewModel.CategoryId,
                Name = viewModel.Name
            };
        }

        public static CategoryViewModel FromCategory(Category model)
        {
            return new CategoryViewModel
            {
                CategoryId = model.CategoryId,
                Name = model.Name
            };
        }
    }
}
