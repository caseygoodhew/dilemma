using System.Data.Entity.ModelConfiguration;

using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Maps
{
    /// <summary>
    /// <see cref="Bookmark"/> entity configuration.
    /// </summary>
    public class BookmarkMap : EntityTypeConfiguration<Bookmark>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkMap" /> class.
        /// </summary>
        public BookmarkMap()
        {
            HasKey(p => p.Id);

            HasRequired(p => p.User).WithMany().WillCascadeOnDelete(false);
            HasRequired(p => p.Question).WithMany().WillCascadeOnDelete(false);

            Property(p => p.CreatedDateTime).IsRequired();
        }
    }
}