using System;

using Dilemma.Business.Validators;
using Dilemma.Common;
using Dilemma.Web.Controllers;
using Dilemma.Web.Validators;
using Dilemma.Web.ViewModels;

using Disposable.Common.ServiceLocator;

using FluentValidation;
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
                    NotificationRoute.Create<AnswersController>(NotificationType.QuestionApproved, x => x.Index(0)),
                    NotificationRoute.Create<AnswersController>(NotificationType.AnswerApproved, x => x.Index(0)),
                    NotificationRoute.Create<AnswersController>(NotificationType.FollowupApproved,  x => x.Index(0)),

                    NotificationRoute.Create<ModerationController>(NotificationType.QuestionRejected, x => x.Details(0)),
                    NotificationRoute.Create<ModerationController>(NotificationType.AnswerRejected, x => x.Details(0)),
                    NotificationRoute.Create<ModerationController>(NotificationType.FollowupRejected, x => x.Details(0)),
        
                    //NotificationRoute.Create<XXX>(NotificationType.FlaggedQuestionApproved,
                    //NotificationRoute.Create<XXX>(NotificationType.FlaggedAnswerApproved,
                    //NotificationRoute.Create<XXX>(NotificationType.FlaggedFollowupApproved,

                    //NotificationRoute.Create<XXX>(NotificationType.FlaggedQuestionRejected,
                    //NotificationRoute.Create<XXX>(NotificationType.FlaggedAnswerRejected,
                    //NotificationRoute.Create<XXX>(NotificationType.FlaggedFollowupRejected,
        
                    NotificationRoute.Create<AnswersController>(NotificationType.OpenForVoting, x => x.Index(0)),
                    NotificationRoute.Create<AnswersController>(NotificationType.VoteOnAnswer, x => x.Index(0)),
                    NotificationRoute.Create<AnswersController>(NotificationType.BestAnswerAwarded, x => x.Index(0))
                ));

            locator.Register<IValidator<AskViewModel>>(() => new AskViewModelValidator());
            locator.Register<IValidator<DilemmaDetailsViewModel>>(() => new DilemmaDetailsViewModelValidator());
        }
    }
}