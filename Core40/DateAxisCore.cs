using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public class DateAxisCore : WindowAxisCore
    {
        private DateTime _referenceDateTime = DateTime.MinValue;
        private SeriesResolution _seriesResolution = SeriesResolution.Ticks;

        public DateAxisCore(IWindowAxisView view) : base(view)
        {
            CleanFactor = 6;
        }

        /// <inheritdoc />
        public override Func<double, string> GetFormatter()
        {
            return FormatLabel;
        }

        internal override CoreMargin PrepareChart(AxisOrientation source, ChartCore chart)
        {
            // Get the current configued values from the view
            _referenceDateTime = ((IDateAxisView) View).ReferenceDateTime;
            _seriesResolution = ((IDateAxisView)View).Resolution;

            return base.PrepareChart(source, chart);
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
            // All our X values are based upon this starting point (configured by the user)
            // Using this starting point, we can calculate the DateTime represented by this X value           
            
            // We use the series resolution (configured by the user) to determine which unit to use to increase the reference date.
            DateTime dateTime;

            switch (_seriesResolution)
            {
                case SeriesResolution.Ticks:
                    dateTime = _referenceDateTime.AddTicks((long)x);
                    break;
                case SeriesResolution.Second:
                    dateTime = _referenceDateTime.AddSeconds(Math.Floor(x));
                    break;
                case SeriesResolution.Minute:
                    dateTime = _referenceDateTime.AddMinutes(Math.Floor(x));
                    break;
                case SeriesResolution.Hour:
                    dateTime = _referenceDateTime.AddHours(Math.Floor(x));
                    break;
                case SeriesResolution.Day:
                    dateTime = _referenceDateTime.AddDays(Math.Floor(x));
                    break;

                default:
                    throw new ArgumentException();
            }

            return dateTime;
        }
    }
}