using System;
using System.Linq.Expressions;

using FluentValidation;
using FluentValidation.Validators;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates that a trimmed value length falls within a given range.
    /// </summary>
    public class TrimmedLengthValidator : LengthValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrimmedLengthValidator"/> class.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public TrimmedLengthValidator(int min, int max)
            : base(min, max)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrimmedLengthValidator"/> class.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="errorMessageResourceSelector">No idea what this is for.</param>
        public TrimmedLengthValidator(int min, int max, Expression<Func<string>> errorMessageResourceSelector)
            : base(min, max, errorMessageResourceSelector)
        {
        }

        /// <summary>
        /// Validates the property.
        /// </summary>
        /// <param name="context">The <see cref="PropertyValidatorContext"/></param>.
        /// <returns>True if the value is valid, false if it is not.</returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            context.PropertyValue = context.PropertyValue == null ? null : context.PropertyValue.ToString().Trim();
            return base.IsValid(context);
        }
    }
}
