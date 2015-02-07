using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class UserPoint
    {
        public int Id { get; set; }
        
        public virtual User ForUser { get; set; }

        public virtual PointType PointType { get; set; }

        public virtual Question RelatedQuestion { get; set; }

        public int PointsAwarded { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
