using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Definitions.Charts;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public class DateAxisCore : WindowAxisCore
    {
        public DateAxisCore(IAxisView view) : base(view)
        {
        }

        /// <inheritdoc />
        public override Func<double, string> GetFormatter()
        {
            return FormatLabel;
        }

        private string FormatLabel(double x)
        {
            // For the points, we use the actual value formatted with its resolution
            var dateTime = GetdateTime(x);

            switch (((IDateAxisView)View).Resolution)
            {
                case SeriesResolution.Second:
                    return dateTime.ToString("dd-MM-yyyy hh:mm:ss");

                case SeriesResolution.Minute:
                    return dateTime.ToString("dd-MM-yyyy hh:mm");

                case SeriesResolution.Hour:
                    return dateTime.ToString("dd-MM-yyyy hh:00");

                case SeriesResolution.Day:
                    return dateTime.ToString("dd-MM-yyyy");

                case SeriesResolution.Ticks:
                    return dateTime.ToString("dd-MM-yyyy hh:mm:ss");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal DateTime GetdateTime(double x)
        {
            // Get the reference date time
            // All our X values are based upon this starting point (configured by the user)
            // Using this starting point, we can calculate the DateTime represented by this X value
            var referenceDateTime = ((IDateAxisView)View).ReferenceDateTime;

            // We use the series resolution (configured by the user) to determine which unit to use to increase the reference date.
            DateTime dateTime;

            switch (((IDateAxisView)View).Resolution)
            {
                case SeriesResolution.Ticks:
                    dateTime = referenceDateTime.AddTicks((long)x);
                    break;
                case SeriesResolution.Second:
                    dateTime = referenceDateTime.AddSeconds(Math.Floor(x));
                    break;
                case SeriesResolution.Minute:
                    dateTime = referenceDateTime.AddMinutes(Math.Floor(x));
                    break;
                case SeriesResolution.Hour:
                    dateTime = referenceDateTime.AddHours(Math.Floor(x));
                    break;
                case SeriesResolution.Day:
                    dateTime = referenceDateTime.AddDays(Math.Floor(x));
                    break;

                default:
                    throw new ArgumentException();
            }

            return dateTime;
        }
    }
}