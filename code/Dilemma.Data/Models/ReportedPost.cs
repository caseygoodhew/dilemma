using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class ReportedPost
    {
        public int Id { get; set; }

        public virtual User ByUser { get; set; }

        public virtual Question Question { get; set; }

        public virtual Answer Answer { get; set; }

        public ReportReason Reason { get; set; }
        
        public DateTime ReportedDateTime { get; set; }
    }
}