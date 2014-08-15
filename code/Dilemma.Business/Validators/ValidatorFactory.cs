using System;

using Disposable.Common.ServiceLocator;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return Locator.Current.Instance(validatorType) as IValidator;
        }
    }
}
