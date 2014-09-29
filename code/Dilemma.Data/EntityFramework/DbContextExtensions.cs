using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Disposable.Common;

namespace Dilemma.Data.EntityFramework
{
    /// <summary>
    /// Extends DbContext functionality.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Calls <see cref="DbContext.SaveChanges"/>, catches and re-throws <see cref="DbEntityValidationException"/> 
        /// after expanding the specific validation errors to the inner exception to make them easy to inspect.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/></param>
        /// <returns>The number of objects written to the underlying database.</returns>
        public static int SaveChangesVerbose(this DbContext context)
        {
            Guard.ArgumentNotNull(context, "context");
            
            try
            {
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(
                    dbEx,
                    (current1, validationErrors) =>
                    validationErrors.ValidationErrors.Select(
                        validationError =>
                        string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage))
                        .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));

                throw new DbEntityValidationException("Repackaged validation errors to InnerException", exception);
            }
        }

        public static TSource EnsureAttached<TSource, TProperty>(this DbContext context, Expression<Func<TSource, TProperty>> propertyLambda, int id) where TSource : class, new()
        {
            var propertyInfo = Reflection.GetPropertyInfo(propertyLambda);

            var options = context.ChangeTracker.Entries<TSource>().Where(
                x =>
                    {
                        var test = (int)propertyInfo.GetValue(x.Entity);
                        return test == id;
                    }).ToList();
            
            if (options.Count == 0)
            {
                var entity = new TSource();
                propertyInfo.SetValue(entity, id);
                return context.Set<TSource>().Attach(entity);
            }

            if (options.Count == 1)
            {
                return options.Single().Entity; 
            }

            throw new InvalidOperationException();
        }

        
    }
}
