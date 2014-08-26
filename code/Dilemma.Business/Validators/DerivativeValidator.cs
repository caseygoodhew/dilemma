using System.Security.Cryptography.X509Certificates;

using Disposable.Common.ServiceLocator;

using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Allows a view model which inherits from a base view model to be validated by the base view model's validator.
    /// </summary>
    /// <typeparam name="TFrom">The assigned type to validate.</typeparam>
    /// <typeparam name="TTo">The base type to validate.</typeparam>
    public class DerivativeValidator<TFrom, TTo> : AbstractValidator<TFrom> where TFrom : TTo
    {
        private readonly IValidator<TTo> toValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivativeValidator{TFrom,TTo}"/> class.
        /// </summary>
        public DerivativeValidator()
        {
            toValidator = Locator.Current.Instance<IValidator<TTo>>();

            AddRule(
                new DelegateValidator<TTo>(
                    (item, context) =>
                        {
                            var result = toValidator.Validate(item);
                            return result.Errors;
                        }));
        }
    }
}
