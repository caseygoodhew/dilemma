using System;

namespace Dilemma.IntegrationTest.ServiceLevel.Support
{
    internal class TimeWarpSource : ITimeWarpSource
    {
        private TimeWarpTo _timeWarpTo = TimeWarpTo.TheHereAndNow;
        
        public DateTime Now 
        {
            get
            {
                switch (_timeWarpTo)
                {
                    case TimeWarpTo.TenYearsAgo:
                        return DateTime.Now.AddYears(-10);
                    case TimeWarpTo.TenMinutesAgo:
                        return DateTime.Now.AddMinutes(-10);
                    case TimeWarpTo.OneMinuteAgo:
                        return DateTime.Now.AddMinutes(-1);
                    case TimeWarpTo.TenSecondsAgo:
                        return DateTime.Now.AddSeconds(-10);
                    case TimeWarpTo.TheHereAndNow:
                        return DateTime.Now;
                    case TimeWarpTo.TenSecondsFromNow:
                        return DateTime.Now.AddSeconds(10);
                    case TimeWarpTo.OneMinuteFromNow:
                        return DateTime.Now.AddMinutes(1);
                    case TimeWarpTo.TenMinutesFromNow:
                        return DateTime.Now.AddMinutes(10);
                    case TimeWarpTo.TenYearsFromNow:
                        return DateTime.Now.AddYears(10);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void DoThe(TimeWarpTo timeWarpTo)
        {
            _timeWarpTo = timeWarpTo;
        }
    }
}