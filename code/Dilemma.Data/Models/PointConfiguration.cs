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

        public int Id
        {
            get
            {
                return (int)PointType;
            }
            
            set
            {
                var type = typeof(PointType);

                if (!Enum.IsDefined(type, value))
                {
                    throw new InvalidOperationException(string.Format("{0} cannot be mapped to a PointType.", value));
                }

                PointType = (PointType)Enum.ToObject(type, value);
            }
        }
            
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
                    throw new InvalidOperationException(string.Format("PointType cannot be changed from {0} ({1}) to {2} ({3})", _pointType, (int)_pointType, value, (int)value));
                }

                _pointType = value;
            }
        }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Points { get; set; }
    }
}
