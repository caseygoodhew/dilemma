﻿using Dilemma.Business.ViewModels;
using Dilemma.Common;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates the <see cref="SystemConfigurationViewModel"/>.
    /// </summary>
    public class SystemConfigurationViewModelValidator : AbstractValidator<SystemConfigurationViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigurationViewModelValidator"/> class.
        /// </summary>
        public SystemConfigurationViewModelValidator()
        {
            RuleFor(x => x.MaxAnswers).InclusiveBetween(1, 10);
            RuleFor(x => x.QuestionLifetimeDays).NotNull().InclusiveBetween(1, 100);
            RuleFor(x => x.SystemEnvironment).NotNull().NotEqual(SystemEnvironment.Testing);
            RuleFor(x => x.RetireQuestionAfterDays).NotNull().GreaterThanOrEqualTo(7);
            RuleFor(x => x.ExpireAnswerSlotsAfterMinutes).NotNull().GreaterThanOrEqualTo(10);
            RuleFor(x => x.EmailErrorsTo).EmailAddress();
        }
    }
}
