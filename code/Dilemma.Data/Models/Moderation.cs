using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class Moderation
    {
        public int ModerationId { get; set; }

        public User ForUser { get; set; }
        
        public ModerationFor ModerationFor { get; set; }
        
        public Question Question { get; set; }

        public Answer Answer { get; set; }

        public ModerationState State { get; set; }

        public ModerationEntry MostRecentEntry { get; set; }

        public virtual List<ModerationEntry> ModerationEntries { get; set; }
    }
}
