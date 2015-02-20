using System;

namespace Dilemma.Data.Models
{
    public class LastRunLog
    {
        /// <summary>
        /// Always 1. There is only one last run log. Getter and setter are implemented for EF compatibility only.
        /// </summary>
        public int Id
        {
            get { return 1; }
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }

        public DateTime? ExpireAnswerSlots { get; set; }

        public DateTime? CloseQuestions { get; set; }

        public DateTime? RetireOldQuestions { get; set; }
    }
}