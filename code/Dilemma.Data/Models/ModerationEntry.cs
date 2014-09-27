using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class ModerationEntry
    {
        public int ModerationEntryId { get; set; }

        public Moderation Moderation { get; set; }

        public ModerationEntryType EntryType { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}