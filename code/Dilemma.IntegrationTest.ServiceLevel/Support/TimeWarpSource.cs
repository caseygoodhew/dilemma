using System;

namespace Dilemma.IntegrationTest.ServiceLevel.Support
{
    internal class TimeWarpSource : ITimeWarpSource
    {
        private TimeWarpTo _timeWarpTo = TimeWarpTo.TheHereAndNow;

        private DateTime? _momentInTime;
        
        public DateTime Now 
        {
            get
            {
                if (_momentInTime.HasValue)
                {
                    return _momentInTime.Value;
                }
                
                switch (_timeWarpTo)
                {
                    case TimeWarpTo.TenYearsAgo:
                        return DateTime.UtcNow.AddYears(-10);
                    case TimeWarpTo.TenMinutesAgo:
                        return DateTime.UtcNow.AddMinutes(-10);
                    case TimeWarpTo.OneMinuteAgo:
                        return DateTime.UtcNow.AddMinutes(-1);
                    case TimeWarpTo.TenSecondsAgo:
                        return DateTime.UtcNow.AddSeconds(-10);
                    case TimeWarpTo.TheHereAndNow:
                        return DateTime.UtcNow;
                    case TimeWarpTo.TenSecondsFromNow:
                        return DateTime.UtcNow.AddSeconds(10);
                    case TimeWarpTo.OneMinuteFromNow:
                        return DateTime.UtcNow.AddMinutes(1);
                    case TimeWarpTo.TenMinutesFromNow:
                        return DateTime.UtcNow.AddMinutes(10);
                    case TimeWarpTo.TenYearsFromNow:
                        return DateTime.UtcNow.AddYears(10);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void DoThe(TimeWarpTo timeWarpTo)
        {
            _timeWarpTo = timeWarpTo;
        }

        public void FreezeTime(DateTime? dateTime = null)
        {
            if (!dateTime.HasValue)
            {
                dateTime = Now;
            }

            _momentInTime = dateTime;
        }

        public void UnFreezeTime()
        {
            _momentInTime = null;
        }
    }
}