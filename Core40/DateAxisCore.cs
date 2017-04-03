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
    /// <seealso cref="LiveCharts.AxisCore" />
    public sealed class DateAxisCore : AxisCore
    {
        private readonly List<DateAxisWindow> _windows;

        private DateAxisWindow ActiveWindow
        {
            get { return _windows.Single(w => w.Resolution == ((IDateAxisView)View).SeparatorResolution); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public DateAxisCore(IAxisView view) : base(view)
        {
            _windows = new List<DateAxisWindow>(DateAxisWindows.GetDefaultWindows());
        }

        /// <summary>
        /// 
        /// </summary>
        internal override CoreMargin PrepareChart(AxisOrientation source, ChartCore chart)
        {
            if (!(Math.Abs(TopLimit - BotLimit) > S * .01) || !ShowLabels) return new CoreMargin();

            // Calculate the resolution of the separators based upon the current window (BotLimit and TopLimit).
            CalculateSeparatorResolution();

            var currentMargin = new CoreMargin();
            var tolerance = S / 10;

            InitializeGarbageCollector();

            FirstSeparator = BotLimit;

            // Loop trough all the X values shown in the window
            for (var x = Math.Floor(BotLimit); x <= TopLimit - (EvaluatesUnitWidth ? 1 : 0); x += 1)
            {

                var dateTime = GetdateTime(x);

                // Skip when this is no seperator for this resolution
                if (!ActiveWindow.IsSeparator.Invoke(dateTime)) continue;

                LastSeparator = x;
                SeparatorElementCore asc;

                var key = Math.Round(x / tolerance) * tolerance;
                if (!Cache.TryGetValue(key, out asc))
                {
                    asc = new DateSeparatorElementCore { IsNew = true };
                    Cache[key] = asc;
                }
                else
                {
                    asc.IsNew = false;
                }

                // Determine whether this separator is a header now
                ((DateSeparatorElementCore)asc).IsHeader = ActiveWindow.IsHeader.Invoke(dateTime);
                
                View.RenderSeparator(asc, Chart);

                asc.Key = key;
                asc.Value = x;
                asc.GarbageCollectorIndex = GarbageCollectorIndex;

                var labelsMargin = asc.View.UpdateLabel(ActiveWindow.AxisLabel.Invoke(dateTime), this, source);

                currentMargin.Width = labelsMargin.TakenWidth > currentMargin.Width
                    ? labelsMargin.TakenWidth
                    : currentMargin.Width;
                currentMargin.Height = labelsMargin.TakenHeight > currentMargin.Height
                    ? labelsMargin.TakenHeight
                    : currentMargin.Height;

                currentMargin.Left = labelsMargin.Left > currentMargin.Left
                    ? labelsMargin.Left
                    : currentMargin.Left;
                currentMargin.Right = labelsMargin.Right > currentMargin.Right
                    ? labelsMargin.Right
                    : currentMargin.Right;

                currentMargin.Top = labelsMargin.Top > currentMargin.Top
                    ? labelsMargin.Top
                    : currentMargin.Top;
                currentMargin.Bottom = labelsMargin.Bottom > currentMargin.Bottom
                    ? labelsMargin.Bottom
                    : currentMargin.Bottom;

                if (LastAxisMax == null)
                {
                    asc.State = SeparationState.InitialAdd;
                    continue;
                }

                asc.State = SeparationState.Keep;
            }
            return currentMargin;
        }
        
        /// <inheritdoc />
        public override Func<double, string> GetFormatter()
        {
            return FormatLabel;
        }

        private void CalculateSeparatorResolution()
        {
            var windowSize = TopLimit - BotLimit;
            SeparatorResolution maximumResolution;

            // Adjust the window to a resolution we use to determine separator resolution
            switch (((IDateAxisView)View).Resolution)
            {
                case SeriesResolution.Ticks:
                    windowSize = windowSize / 24 / 60 / 60 / 100;
                    maximumResolution = SeparatorResolution.Second;
                    break;

                case SeriesResolution.Second:
                    windowSize = windowSize / 24 / 60 / 60;
                    maximumResolution = SeparatorResolution.Second;
                    break;

                case SeriesResolution.Minute:
                    windowSize = windowSize / 24 / 60;
                    maximumResolution = SeparatorResolution.Minute;
                    break;
                case SeriesResolution.Hour:
                    windowSize = windowSize / 24;
                    maximumResolution = SeparatorResolution.Hour;
                    break;
                case SeriesResolution.Day:
                    maximumResolution = SeparatorResolution.Day;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Find the seperator resolution represented by the first available window
            var separatorResolution = _windows
                .Where(w => w.Resolution >= maximumResolution)
                .First(w => w.Threshold >= windowSize)
                .Resolution;
                
            ((IDateAxisView)View).SetSeparatorResolution(separatorResolution);
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

        private DateTime GetdateTime(double x)
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