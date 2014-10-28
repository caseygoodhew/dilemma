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

        /// <summary>
        /// Checks the context's local dictionary for entities of type <see cref="TSource"/> that have a 
        /// property as defined by <see cref="propertyLambda"/> with a value of <see cref="value"/>. 
        /// <see cref="propertyLambda"/> will typically be the entity's primary key but could be any 
        /// single unique identifier. If no match is found, a new instance of TSource will 
        /// be created with it's property set to value and attached to the context.
        /// </summary>
        /// <typeparam name="TSource">The model type to search for.</typeparam>
        /// <typeparam name="TProperty">The type of the property to check.</typeparam>
        /// <param name="context">The <see cref="DbContext"/> to extend.</param>
        /// <param name="value">The value to check for.</param>
        /// <param name="propertyLambda">A property expression.</param>
        /// <returns>The attached model object.</returns>
        public static TSource GetOrAttachNew<TSource, TProperty>(
            this DbContext context,
            TProperty value,
            Expression<Func<TSource, TProperty>> propertyLambda) where TSource : class, new()
        {
            var propertyInfo = Reflection.GetPropertyInfo(propertyLambda);

            var existingEntity = context.ChangeTracker.Entries<TSource>().Where(
                x =>
                {
                    var propertyValue = (TProperty)propertyInfo.GetValue(x.Entity);
                    return EqualityComparer<TProperty>.Default.Equals(propertyValue, value);
                }).SingleOrDefault();

            if (existingEntity != null)
            {
                return existingEntity.Entity;
            }

            var newObj = new TSource();
            propertyInfo.SetValue(newObj, value);
            context.Set<TSource>().Attach(newObj);

            return newObj;
        }

        /// <summary>
        /// Checks the context's local dictionary for entities of type <see cref="TSource"/> that have a 
        /// property as defined by <see cref="propertyLambda"/> with a value of the corresponding value
        /// as is provided by <see cref="source"/>. If a match is found, but the attached object is not
        /// the <see cref="source"/> object, a <see cref="AreNotSameException"/> will be thrown. This is
        /// to prevent potential data skew between context model objects. If no matching model object is
        /// found, the provided <see cref="source"/> object will be attached to the context.
        /// </summary>
        /// <typeparam name="TSource">The model type to search for.</typeparam>
        /// <typeparam name="TProperty">The type of the property to check.</typeparam>
        /// <param name="context">The <see cref="DbContext"/> to extend.</param>
        /// <param name="source">The model object to search for or attach.</param>
        /// <param name="propertyLambda">A property expression.</param>
        /// <returns>The attached model object.</returns>
        public static TSource EnsureAttached<TSource, TProperty>(
            this DbContext context,
            TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda) where TSource : class
        {
            var propertyInfo = Reflection.GetPropertyInfo(propertyLambda);
            var value = (TProperty)propertyInfo.GetValue(source);
            
            var existingEntity = context.ChangeTracker.Entries<TSource>().Where(
                x =>
                {
                    var propertyValue = (TProperty)propertyInfo.GetValue(x.Entity);
                    return EqualityComparer<TProperty>.Default.Equals(propertyValue, value);
                }).SingleOrDefault();
            
            if (existingEntity != null)
            {
                if (existingEntity.Entity == source)
                {
                    return source;
                }

                throw new AreNotSameException();
            }

            context.Set<TSource>().Attach(source);

            return source;
        }
    }
}
