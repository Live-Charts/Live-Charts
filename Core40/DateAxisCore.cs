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
        private DateTime _initialDateTime = DateTime.MinValue;
        private PeriodUnits _period = PeriodUnits.Ticks;

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
            _initialDateTime = ((IDateAxisView) View).InitialDateTime;
            _period = ((IDateAxisView)View).Period;

            return base.PrepareChart(source, chart);
        }

        private string FormatLabel(double x)
        {
            // For the points, we use the actual value based upon the period
            var dateTime = GetdateTime(x);

            switch (((IDateAxisView)View).Period)
            {
                case PeriodUnits.Seconds:
                    return dateTime.ToString("dd-MM-yyyy hh:mm:ss");

                case PeriodUnits.Minutes:
                    return dateTime.ToString("dd-MM-yyyy hh:mm");

                case PeriodUnits.Hours:
                    return dateTime.ToString("dd-MM-yyyy hh:00");

                case PeriodUnits.Days:
                    return dateTime.ToString("dd-MM-yyyy");

                case PeriodUnits.Ticks:
                    return dateTime.ToString("dd-MM-yyyy hh:mm:ss");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal DateTime GetdateTime(double x)
        {
            // All our X values are based upon this starting point (configured by the user)
            // Using this starting point, we can calculate the DateTime represented by this X value           
            
            // We use the series period (configured by the user) to determine which period unit to use to increase the reference date.
            DateTime dateTime;

            switch (_period)
            {
                case PeriodUnits.Ticks:
                    dateTime = _initialDateTime.AddTicks((long)x);
                    break;
                case PeriodUnits.Seconds:
                    dateTime = _initialDateTime.AddSeconds(Math.Floor(x));
                    break;
                case PeriodUnits.Minutes:
                    dateTime = _initialDateTime.AddMinutes(Math.Floor(x));
                    break;
                case PeriodUnits.Hours:
                    dateTime = _initialDateTime.AddHours(Math.Floor(x));
                    break;
                case PeriodUnits.Days:
                    dateTime = _initialDateTime.AddDays(Math.Floor(x));
                    break;

                default:
                    throw new ArgumentException();
            }

            return dateTime;
        }
    }
}