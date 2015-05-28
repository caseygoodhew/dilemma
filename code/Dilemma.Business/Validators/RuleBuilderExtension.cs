using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Applies custom validators to the rule builder.
    /// </summary>
    public static class RuleBuilderExtension
    {
        /// <summary>
        /// Exposes the <see cref="TrimmedLengthValidator"/> to the fluent validation chain.
        /// </summary>
        /// <typeparam name="T">The type to validate.</typeparam>
        /// <param name="ruleBuilder">The rule builder (self).</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The rule builder (self).</returns>
        public static IRuleBuilderOptions<T, string> TrimmedLength<T>(this IRuleBuilder<T, string> ruleBuilder, int min, int max)
        {
            return ruleBuilder.SetValidator(new TrimmedLengthValidator(min, max));
        }

        /// <summary>
        /// Exposes the <see cref="TidiedTrimmedLengthValidator"/> to the fluent validation chain.
        /// </summary>
        /// <typeparam name="T">The type to validate.</typeparam>
        /// <param name="ruleBuilder">The rule builder (self).</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The rule builder (self).</returns>
        public static IRuleBuilderOptions<T, string> TidiedTrimmedLength<T>(this IRuleBuilder<T, string> ruleBuilder, int min, int max)
        {
            return ruleBuilder.SetValidator(new TidiedTrimmedLengthValidator(min, max));
        }

        public static IRuleBuilderOptions<T, string> WebPurify<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new WebPurifyValidator());
        }
    }
}