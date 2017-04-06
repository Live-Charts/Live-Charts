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
        /// Gets preconfigured windows that can be used to build a datetime window axis
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DateAxisWindow> GetDateAxisWindows()
        {
            yield return new SecondAxisWindow();
            yield return new FifteenSecondsAxisWindow();
            yield return new ThirtySecondsAxisWindow();
            yield return new MinuteAxisWindow();
            yield return new QuarterAxisWindow();
            yield return new HalfHourAxisWindow();
            yield return new HourAxisWindow();
            yield return new DayAxisWindow();
            yield return new WeekAxisWindow();
            yield return new MonthAxisWindow();
            yield return new YearAxisWindow();
            yield return new DecadeAxisWindow();
            yield return new CenturyAxisWindow();
            yield return new MillenniumAxisWindow();
        }

        /// <inheritdoc />
        public sealed class SecondAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 20; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Second == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsSecond(x);
            }
            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString(IsHeader(x)
                    ? "hh:mm:ss"
                    : "mm:ss");
            }        

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalSeconds <= 1;
            }
        }

        /// <inheritdoc />
        public sealed class FifteenSecondsAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 50; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Minute == 0 && x.Second == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsSecond(x) && x.Second % 15 == 0;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString("hh:mm:ss");
            }

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalSeconds <= 15;
            }
        }

        /// <inheritdoc />
        public sealed class ThirtySecondsAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 40; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Minute == 0 && x.Second == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsSecond(x) && x.Second % 30 == 0;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString("hh:mm:ss");
            }

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalSeconds <= 30;
            }
        }

        /// <inheritdoc />
        public sealed class MinuteAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 20; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Hour == 0 && x.Minute == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsMinute(x);
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString("hh:mm");
            }

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalMinutes <= 1;
            }
        }

        /// <inheritdoc />
        public sealed class QuarterAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 20; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Minute == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsMinute(x) && x.Minute % 15 == 0;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.Hour == 0 && x.Minute == 0
                    ? x.ToString("d")
                    : x.ToString("hh:mm");
            }

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalMinutes <= 15;
            }
        }

        /// <inheritdoc />
        public sealed class HalfHourAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 20; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Hour == 0 && x.Minute == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsMinute(x) && x.Minute % 30 == 0;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return IsHeader(x)
                    ? x.ToString("d")
                    : x.ToString("hh:mm");
            }

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalMinutes <= 30;
            }
        }

        /// <inheritdoc />
        public sealed class HourAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 20; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Hour == 0 && x.Minute == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsHour(x);
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return IsHeader(x)
                    ? x.ToString("d")
                    : x.ToString("hh:mm");
            }

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalHours <= 1;
            }
        }

        /// <inheritdoc />
        public sealed class DayAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 10; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.DayOfYear == 1 || x.Day == 1;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsDay(x);
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.DayOfYear == 1
                    ? x.ToString("yyyy")
                    : x.ToString(IsHeader(x)
                        ? "MMM"
                        : "dd");
            }

            /// <inheritdoc />
            protected override bool Validate(TimeSpan seperatorDistance)
            {
                return seperatorDistance.TotalDays <= 1;
            }
        }

        /// <inheritdoc />
        public sealed class WeekAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 10; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                // TODO: See IsSeparator
                return x.Day <= 7;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                // TODO: First day of week is not always monday.
                // Use CultureInfo to determine the first day of week
                return IsDay(x) && x.DayOfWeek == DayOfWeek.Monday;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString(IsHeader(x)
                    ? "MMM"
                    : "dd");
            }
        }

        /// <inheritdoc />
        public sealed class MonthAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 10; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.DayOfYear == 1;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsDay(x) && x.Day == 1;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString(IsHeader(x)
                    ? "yyyy"
                    : "MMM");
            }
        }

        /// <inheritdoc />
        public sealed class YearAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 20; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Year % 10 == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsYear(x);
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString("yyyy");
            }
        }

        /// <inheritdoc />
        public sealed class DecadeAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 10; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return x.Year % 100 == 0;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsYear(x) && x.Year % 10 == 0;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString("yyyy");
            }
        }

        /// <inheritdoc />
        public sealed class CenturyAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 10; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return false;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsYear(x) && x.Year % 100 == 0;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString("yyyy");
            }
        }

        /// <inheritdoc />
        public sealed class MillenniumAxisWindow : DateAxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth
            {
                get { return 10; }
            }

            /// <inheritdoc />
            public override bool IsHeader(DateTime x)
            {
                return false;
            }

            /// <inheritdoc />
            public override bool IsSeparator(DateTime x)
            {
                return IsYear(x) && x.Year % 1000 == 0;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(DateTime x)
            {
                return x.ToString("yyyy");
            }
        }
    }
}