using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Dilemma.ViewModels.Validation;

using Disposable.Common.ServiceLocator;

using FluentValidation.Mvc;

namespace Dilemma.Web
{
    /// <summary>
    /// Static registration entry point for IOC registration
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Static registration entry point for IOC registration
        /// </summary>
        /// <param name="locator">The locator</param>
        public static void Register(ILocator locator)
        {
            // TODO: Is this correct? (it's not using the locator)
            FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new ValidatorFactory());
        }
    }
}