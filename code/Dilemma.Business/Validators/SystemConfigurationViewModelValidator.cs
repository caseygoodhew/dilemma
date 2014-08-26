using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Business.ViewModels;

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
            RuleFor(x => x.QuestionLifetime).NotNull();
            RuleFor(x => x.SystemEnvironment).NotNull();
        }
    }
}
