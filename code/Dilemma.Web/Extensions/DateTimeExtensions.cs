using System;

using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Web.Extensions
{
    public static class DateTimeExtensions
    {
        const int Second = 1;
        const int Minute = 60 * Second;
        const int Hour = 60 * Minute;
        const int Day = 24 * Hour;
        const int Month = 30 * Day;

        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        public static string ToRelativeText(this DateTime? dateTime, string whenNull = "")
        {
            return dateTime.HasValue ? dateTime.Value.ToRelativeText() : whenNull;
        }
        
        public static string ToRelativeText(this DateTime dateTime)
        {
            var ts = new TimeSpan(TimeSource.Value.Now.Ticks - dateTime.Ticks);
            var delta = ts.TotalSeconds;
            int years;
            
            if (delta < 0)
            {
                delta = delta * -1;
                ts = new TimeSpan(0, 0, Convert.ToInt32(delta));

                if (delta < 1 * Minute)
                {
                    return "Now";
                }

                if (delta < 2 * Minute)
                {
                    return "in one minute";
                }

                if (delta < 45 * Minute)
                {
                    return "in " + ts.Minutes + " minutes";
                }

                if (delta < 120 * Minute)
                {
                    return "in an hour";
                }

                if (delta < 23 * Hour)
                {
                    return "in " + ts.Hours + " hours";
                }

                if (delta < 48 * Hour)
                {
                    return "tomorrow";
                }

                if (delta < 30 * Day)
                {
                    return "in " + ts.Days + " days";
                }

                if (delta < 12 * Month)
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? "in one month" : "in " + months + " months";
                }

                years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "in one year" : "in " + years + " years";
            }

            if (delta < 1 * Minute)
            {
                return "Just Now";
            }

            if (delta < 2 * Minute)
            {
                return "a minute ago";
            }

            if (delta < 45 * Minute)
            {
                return ts.Minutes + " minutes ago";
            }

            if (delta < 120 * Minute)
            {
                return "an hour ago";
            }

            if (delta < 24 * Hour)
            {
                return ts.Hours + " hours ago";
            }

            if (delta < 48 * Hour)
            {
                return "yesterday";
            }

            if (delta < 30 * Day)
            {
                return ts.Days + " days ago";
            }

            if (delta < 12 * Month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            
            years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}