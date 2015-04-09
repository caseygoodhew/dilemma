using System.Collections.Generic;

namespace Dilemma.Data.Models.Proxies
{
    public class QuestionHistoryProxy
    {
        public Question Question { get; set; }

        public IEnumerable<ModerationEntry> ModerationEntries { get; set; }
    }
}