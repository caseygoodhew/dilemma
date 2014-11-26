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
    /// Validates the <see cref="PointConfigurationViewModel"/>.
    /// </summary>
    public class PointConfigurationViewModelValidator : AbstractValidator<PointConfigurationViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointConfigurationViewModelValidator"/> class.
        /// </summary>
        public PointConfigurationViewModelValidator()
        {
            RuleFor(x => x.PointType).NotNull();
            RuleFor(x => x.Name).TidiedTrimmedLength(4, 100);
            RuleFor(x => x.Description).TidiedTrimmedLength(20, 500);
            RuleFor(x => x.Points).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
