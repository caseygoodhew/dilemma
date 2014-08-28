using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Category"/> entity configuration.
    /// </summary>
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryMap" /> class.
        /// </summary>
        public CategoryMap()
        {
            HasKey(p => p.CategoryId);
            Property(p => p.Name).IsRequired().HasMaxLength(100);
        }
    }
}
