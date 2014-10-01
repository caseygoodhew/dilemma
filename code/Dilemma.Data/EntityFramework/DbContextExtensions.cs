using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

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

        public static TSource EnsureAttached<TSource, TProperty>(
            this DbContext context,
            TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda,
            EntityState entityState = EntityState.Unchanged) where TSource : class
        {
            var propertyInfo = Reflection.GetPropertyInfo(propertyLambda);
            var id = (TProperty)propertyInfo.GetValue(source);
            
            var existingEntity = context.ChangeTracker.Entries<TSource>().Where(
                x =>
                {
                    var propertyValue = (TProperty)propertyInfo.GetValue(x.Entity);
                    return EqualityComparer<TProperty>.Default.Equals(propertyValue, id);
                }).SingleOrDefault();
            
            if (existingEntity != null)
            {
                if (existingEntity.Entity == source)
                {
                    return existingEntity.Entity; 
                }

                if (existingEntity.State != EntityState.Unchanged)
                {
                    throw new AreNotSameException();    
                }

                existingEntity.State = EntityState.Detached;
            }

            context.Set<TSource>().Attach(source);
            context.Entry(source).State = entityState;

            return source;
        }
    }
}
