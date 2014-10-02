using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class ModerationEntry
    {
        public int ModerationEntryId { get; set; }

        public Moderation Moderation { get; set; }

        public ModerationState State { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public User AddedByUser { get; set; }
    }
}