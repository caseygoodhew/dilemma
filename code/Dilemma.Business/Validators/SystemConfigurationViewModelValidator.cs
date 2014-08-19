using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    public class SystemConfigurationViewModelValidator : AbstractValidator<SystemConfigurationViewModel>
    {
        public SystemConfigurationViewModelValidator()
        {
            RuleFor(x => x.MaxAnswers).InclusiveBetween(1, 10);
            RuleFor(x => x.QuestionLifetime).NotNull();
            RuleFor(x => x.SystemEnvironment).NotNull();
        }
    }
}
