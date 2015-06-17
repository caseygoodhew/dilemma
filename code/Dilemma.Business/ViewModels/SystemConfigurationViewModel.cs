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
        public int QuestionLifetimeDays { get; set; }

        /// <summary>
        /// Gets or sets the system environment.
        /// </summary>
        public SystemEnvironment SystemEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the number of days that a question retires after it closes.
        /// </summary>
        public int RetireQuestionAfterDays { get; set; }

        /// <summary>
        /// Gets or sets the number of minutes until an answer slot is expired (deleted) after it was last touched.
        /// </summary>
        public int ExpireAnswerSlotsAfterMinutes { get; set; }

        public bool EnableWebPurify { get; set; }

        public bool EmailErrors { get; set; }

        public string EmailErrorsTo { get; set; }
    }
}

    
