using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Disposable.Common;

namespace Dilemma.Data.EntityFramework
{
    internal static class DbSetExtensions
    {
        internal static void Update<T>(this DbSet<T> dbSet, DbContext context, T entity) where T : class
        {
            Guard.ArgumentNotNull(dbSet, "dbSet");
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
