using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates the <see cref="FollowupViewModel"/>.
    /// </summary>
    public class FollowupViewModelValidator : AbstractValidator<FollowupViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowupViewModelValidator"/> class.
        /// </summary>
        public FollowupViewModelValidator()
        {
            RuleFor(x => x.Text).NotNull().TidiedTrimmedLength(20, 500);
        }
    }
}