using System;

using Dilemma.Common;

namespace Dilemma.Business.ViewModels
{
    public class ModerationEntryViewModel
    {
        public ModerationState State { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string Message { get; set; }
    }
}