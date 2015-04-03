using System.Linq;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Initialization
{
    /// <summary>
    /// Seeds the <see cref="Category"/>s in the database.
    /// </summary>
    internal static class CategoryInitialization
    {
        /// <summary>
        /// Seeds the <see cref="Category"/>s in the database.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/> to seed.</param>
        internal static void Seed(DilemmaContext context)
        {
            var categories = new[] { "Relationship", "Teen", "Work", "Family", "Personal", "Sex" };

            foreach (var category in categories.Where(category => !context.Categories.Any(x => x.Name == category)))
            {
                context.Categories.Add(new Category { Name = category });
            }
        }
    }
}
