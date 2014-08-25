using System;
using System.Linq.Expressions;

using Disposable.Common.Extensions;

using FluentValidation;
using FluentValidation.Validators;

namespace Dilemma.Business.Validators
{
    public class TidiedTrimmedLengthValidator : LengthValidator
    {
        public TidiedTrimmedLengthValidator(int min, int max) : base(min, max)
        {
        }

        public TidiedTrimmedLengthValidator(int min, int max, Expression<Func<string>> errorMessageResourceSelector)
            : base(min, max, errorMessageResourceSelector)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            context.PropertyValue = context.PropertyValue == null ? null : context.PropertyValue.ToString().TidyWhiteSpace().Trim();
            return base.IsValid(context);
        }
    }

    public static partial class RuleBuilderExtension
    {
        public static IRuleBuilderOptions<T, string> TidiedTrimmedLength<T>(this IRuleBuilder<T, string> ruleBuilder, int min, int max)
        {
            return ruleBuilder.SetValidator(new TidiedTrimmedLengthValidator(min, max));
        }
    }
}
