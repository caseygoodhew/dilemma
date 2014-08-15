using System.Security.Cryptography.X509Certificates;

using Disposable.Common.ServiceLocator;

using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace Dilemma.Business.Validators
{
    public class DerivativeValidator<TFrom, TTo> : AbstractValidator<TFrom> where TFrom : TTo
    {
        private readonly IValidator<TTo> toValidator;
        
        public DerivativeValidator()
        {
            toValidator = Locator.Current.Instance<IValidator<TTo>>();

            AddRule(new DelegateValidator<TTo>((item, context) => {
                var result = toValidator.Validate(item);
                return result.Errors;
            }));
        }
    }
}
