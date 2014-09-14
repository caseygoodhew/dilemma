using System;

using Disposable.Common.ServiceLocator;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Adaptor to allow FluentValidation to load validators from the service <see cref="Locator"/>.
    /// </summary>
    public class ValidatorFactory : ValidatorFactoryBase
    {
        /// <summary>
        /// Implementation override of CreateInstance to get validators from the service <see cref="Locator."/>
        /// </summary>
        /// <param name="validatorType">The type of <see cref="IValidator"/> to get.</param>
        /// <returns>The <see cref="IValidator"/> instance, or null.</returns>
        public override IValidator CreateInstance(Type validatorType)
        {
            object instance;

            if (Locator.Current.TryGetInstance(validatorType, out instance))
            {
                return instance as IValidator;
            }

            return null;
        }
    }
}
