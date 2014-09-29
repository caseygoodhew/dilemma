using Dilemma.Business.Validators;
using Dilemma.Common;
using Dilemma.Web.Controllers;

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
        public static void Register(Locator locator)
        {
            // TODO: Is this correct? (it's not using the locator)
            FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new ValidatorFactory());

            locator.Register<INotificationTypeLocator>(() => new NotificationTypeLocator(
                    NotificationRoute.Create<QuestionController>(NotificationType.QuestionAnswered, x => x.Details(0)),
                    NotificationRoute.Create<ModerationController>(NotificationType.PostRejected, x => x.Details(0))
                ));
        }
    }
}