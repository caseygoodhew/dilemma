using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Disposable.Common;

namespace Dilemma.Data.EntityFramework
{
    /// <summary>
    /// Extends DbSet functionality.
    /// </summary>
    internal static class DbSetExtensions
    {
        /// <summary>
        /// Prepares an entity to be updated in the db without first requiring fetch.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="dbSet">The <see cref="DbSet"/>.</param>
        /// <param name="context">The <see cref="DbContext"/> to perform the update against.</param>
        /// <param name="entity">The entity to update.</param>
        internal static void Update<T>(this DbSet<T> dbSet, DbContext context, T entity) where T : class
        {
            Guard.ArgumentNotNull(dbSet, "dbSet");
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
