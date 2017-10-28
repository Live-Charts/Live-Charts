//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
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
        private PeriodUnits _period = PeriodUnits.Milliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAxisCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public DateAxisCore(IWindowAxisView view) : base(view)
        {
            CleanFactor = 3;
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
                    return dateTime.ToString("G");

                case PeriodUnits.Minutes:
                    return dateTime.ToString("g");

                case PeriodUnits.Hours:
                    return dateTime.ToString("g");

                case PeriodUnits.Days:
                    return dateTime.ToString("d");

                case PeriodUnits.Milliseconds:
                    return dateTime.ToString("G") + dateTime.ToString(".fff");

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
                case PeriodUnits.Milliseconds:
                    dateTime = _initialDateTime.AddMilliseconds(Math.Floor(x));
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