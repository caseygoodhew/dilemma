using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Disposable.Common;

namespace Dilemma.Data.EntityFramework
{
    public static class DbContextExtensions
    {
        public static int SaveChangesVerbose(this DbContext context)
        {
            Guard.ArgumentNotNull(context, "context");
            
            try
            {
                return context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(
                    dbEx,
                    (current1, validationErrors) =>
                    validationErrors.ValidationErrors.Select(
                        validationError =>
                        string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage))
                        .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            }
        }
    }
}
