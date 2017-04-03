using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateAxisWindows
    {
        /// <summary>
        /// Gets an array with the default resolutions.
        /// </summary>
        /// <remarks>In the future, I do not expect all existing windows to be default supported (i.e. Milennium, half minutes etc)</remarks>
        public static SeparatorResolution[] DefaultResolutions
        {
            get
            {
                return new[]
                {
                    SeparatorResolution.Second,
                    SeparatorResolution.Minute,
                    SeparatorResolution.Hour,
                    SeparatorResolution.Day,
                    SeparatorResolution.Week,
                    SeparatorResolution.Month,
                    SeparatorResolution.Year,
                    SeparatorResolution.Decade
                };
            }
        }

        /// <summary>
        /// Gets the default configured window for the provided resolution
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateAxisWindow GetDefaultWindow(SeparatorResolution resolution)
        {
            switch (resolution)
            {
                case SeparatorResolution.Second:
                    return SecondWindow;
                    
                case SeparatorResolution.Minute:
                    return MinuteWindow;

                case SeparatorResolution.Hour:
                    return HourWindow;

                case SeparatorResolution.Day:
                    return DayWindow;

                case SeparatorResolution.Week:
                    return WeekWindow;
                case SeparatorResolution.Month:
                    return MonthWindow;
                    
                case SeparatorResolution.Year:
                    return YearWindow;
                    
                case SeparatorResolution.Decade:
                    return DecadeWindow;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets an enumerable of Default Windows based upon the default resolutions
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DateAxisWindow> GetDefaultWindows()
        {
            return GetWindows(DefaultResolutions);
        }

        public static IEnumerable<DateAxisWindow> GetWindows(SeparatorResolution[] resolutions)
        {
            return (Enum.GetValues(typeof(SeparatorResolution))
                .Cast<SeparatorResolution>()
                .Where(resolutions.Contains)
                .Select(GetDefaultWindow)).ToList();
        }

        public static DateAxisWindow SecondWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Second,
            Threshold = 0.005,
            IsHeader = x => x.Second == 0,
            IsSeparator = x => x.Millisecond == 0,
            AxisLabel = x => x.ToString(x.Second == 0 ? "hh:mm:ss" : "ss"),
        };

        public static DateAxisWindow MinuteWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Minute,
            Threshold = 0.03,
            IsHeader = x => x.Minute == 0,
            IsSeparator = x => x.Second == 0,
            AxisLabel = x => x.ToString(x.Minute == 0 ? "hh:mm" : "mm"),
        };

        public static DateAxisWindow HourWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Hour,
            Threshold = 1.2,
            IsHeader = x => x.Hour == 0 && x.Minute == 0,
            IsSeparator = x => x.Minute == 0,
            AxisLabel = x => x.Hour == 0 && x.Minute == 0 ? x.ToString("d") : x.ToString("hh"),
        };

        public static DateAxisWindow DayWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Day,
            Threshold = 30,
            IsHeader = x => x.DayOfYear == 1 || x.Day == 1,
            IsSeparator = x => x.Hour == 0,
            AxisLabel = x => x.DayOfYear == 1 ? x.ToString("yyyy") : x.ToString(x.Day == 1 ? "MMM" : "dd"),
        };

        public static DateAxisWindow WeekWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Week,
            Threshold = 90,
            IsHeader = x => x.Day <= 7,
            IsSeparator = x => x.DayOfWeek == DayOfWeek.Monday,
            AxisLabel = x => x.ToString(x.Day <= 7 ? "MMM" : "dd"),
        };

        public static DateAxisWindow MonthWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Month,
            Threshold = 725,
            IsHeader = x => x.DayOfYear == 1,
            IsSeparator = x => x.Day == 1,
            AxisLabel = x => x.ToString(x.DayOfYear == 1 ? "yyyy" : "MMM"),
        };

        public static DateAxisWindow YearWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Year,
            Threshold = 10950,
            IsHeader = x => false,
            IsSeparator = x => x.DayOfYear == 1,
            AxisLabel = x => x.ToString("yyyy"),
        };

        public static DateAxisWindow DecadeWindow = new DateAxisWindow
        {
            Resolution = SeparatorResolution.Decade,
            Threshold = 10000000,
            IsHeader = x => x.Year % 100 == 0,
            IsSeparator = x => x.Year % 10 == 0,
            AxisLabel = x => x.ToString(x.Year % 100 == 0 ? "yyyy" : "yy"),
        };
    }
}