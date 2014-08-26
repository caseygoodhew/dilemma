using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="CategoryViewModel"/>.
    /// </summary>
    public static class CategoryViewModelConverter
    {
        /// <summary>
        /// Converts a <see cref="CategoryViewModel"/> to a <see cref="Category"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="CategoryViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="Category"/>.</returns>
        public static Category ToCategory(CategoryViewModel viewModel)
        {
            return new Category
            {
                CategoryId = viewModel.CategoryId,
                Name = viewModel.Name
            };
        }

        /// <summary>
        /// Converts a <see cref="Category"/> to a <see cref="CategoryViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Category"/> to convert.</param>
        /// <returns>The resultant <see cref="CategoryViewModel"/>.</returns>
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
