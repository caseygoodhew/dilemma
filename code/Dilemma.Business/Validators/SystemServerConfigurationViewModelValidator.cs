using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.ServiceLocator;

using FluentValidation;
using FluentValidation.Internal;

namespace Dilemma.Business.Validators
{
    public class SystemServerConfigurationViewModelValidator : AbstractValidator<SystemServerConfigurationViewModel>
    {
        public SystemServerConfigurationViewModelValidator()
        {
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