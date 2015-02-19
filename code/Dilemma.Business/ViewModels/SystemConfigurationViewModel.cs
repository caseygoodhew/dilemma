using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The system configuration view model.
    /// </summary>
    public class SystemConfigurationViewModel
    {
        /// <summary>
        /// Gets or sets the maximum number of answers allowed for new questions.
        /// </summary>
        public int MaxAnswers { get; set; }

        /// <summary>
        /// Gets or sets the amount of time that a question should remain open from it's creation date.
        /// </summary>
        public QuestionLifetime QuestionLifetime { get; set; }

        /// <summary>
        /// Gets or sets the system environment.
        /// </summary>
        public SystemEnvironment SystemEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the number of days that a question retires after it closes.
        /// </summary>
        public int RetireQuestionAfterDays { get; set; }
    }
}

    
