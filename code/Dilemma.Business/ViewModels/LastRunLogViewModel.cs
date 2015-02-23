using System;

namespace Dilemma.Business.ViewModels
{
    public class LastRunLogViewModel
    {
        public DateTime? ExpireAnswerSlots { get; set; }

        public DateTime? CloseQuestions { get; set; }

        public DateTime? RetireOldQuestions { get; set; }
    }
}