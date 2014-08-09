using System;

using Disposable.Common.ServiceLocator;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            object validator;
            if (Locator.Current.TryGetInstance(validatorType, out validator))
            {
                return validator as IValidator;
            }

            return null;
        }
    }
}
