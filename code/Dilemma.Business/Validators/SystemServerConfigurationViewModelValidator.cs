using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;

using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace Dilemma.Business.Validators
{
    public class SystemServerConfigurationViewModelValidator : AbstractValidator<SystemServerConfigurationViewModel>
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();
        
        public SystemServerConfigurationViewModelValidator()
        {
            AddRule(
                new DelegateValidator<SystemServerConfigurationViewModel>(
                    (item, context) =>
                        {
                            var currentSettings = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();

                            if (currentSettings.IsInternalEnvironment)
                            {
                                return Enumerable.Empty<ValidationFailure>();
                            }

                            if (item.SystemConfigurationViewModel.SystemEnvironment != SystemEnvironment.Production)
                            {
                                var validationFailure = new ValidationFailure(
                                    "SystemConfigurationViewModel.SystemEnvironment",
                                    "Once the SystemEnvironment is set to production, it can only be altered by a database administrator.");

                                return new[] { validationFailure };
                            }

                            if (!SecurityManager.Value.IsValidAccessKey(item.AccessKey))
                            {
                                var validationFailure = new ValidationFailure(
                                    "AccessKey",
                                    "Invalid AccessKey");

                                return new[] { validationFailure };
                            }

                            return Enumerable.Empty<ValidationFailure>();
                        }));
            
            AddRule(new DelegateValidator<SystemServerConfigurationViewModel>((item, context) =>
                {
                    var result = Locator.Get<IValidator<SystemConfigurationViewModel>>().Validate(item.SystemConfigurationViewModel);
                    return result.Errors;
                }));

            AddRule(new DelegateValidator<SystemServerConfigurationViewModel>((item, context) =>
                {
                    var result = Locator.Get<IValidator<ServerConfigurationViewModel>>().Validate(item.ServerConfigurationViewModel);
                    return result.Errors;
                }));
        }
    }
}