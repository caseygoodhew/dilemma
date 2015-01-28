using Dilemma.Business.ViewModels;

using FluentValidation;

namespace Dilemma.Business.Validators
{
    /// <summary>
    /// Validates the <see cref="ServerConfigurationViewModel"/>.
    /// </summary>
    public class ServerConfigurationViewModelValidator : AbstractValidator<ServerConfigurationViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfigurationViewModelValidator"/> class.
        /// </summary>
        public ServerConfigurationViewModelValidator()
        {
            // nothing to do 
        }
    }
}