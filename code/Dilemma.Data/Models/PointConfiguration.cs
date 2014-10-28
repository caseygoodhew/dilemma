using System;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The point configuration model.
    /// </summary>
    public class PointConfiguration
    {
        private PointType? _pointType;
        
        public int Id { get; set; }
            
        public PointType PointType
        {
            get
            {
                if (!_pointType.HasValue)
                {
                    throw new InvalidOperationException("PointType not set");
                }

                return _pointType.Value;
            }

            set
            {
                if (_pointType.HasValue && _pointType.Value != value)
                {
                    throw new InvalidOperationException(string.Format("PointType cannot be changed from {0} to {1}", _pointType, value));
                }

                _pointType = value;
            }
        }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Points { get; set; }
    }
}
