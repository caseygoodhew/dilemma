using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            HasKey(p => p.CategoryId);
            Property(p => p.Name).IsRequired().HasMaxLength(100);
        }
    }
}
