using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Helpers;

namespace LiveCharts
{
    public class DateAxisCore : WindowAxisCore
    {
        public DateAxisCore(IAxisView view) : base(view)
        {
            // Configure the windows with the default date windows
            var windows = DateAxisWindows.GetDateAxisWindows().ToList();
            windows.ForEach(w => w.DateAxisCore = this); 
            Windows = new List<IAxisWindow>(windows);            
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

    /// <summary>
    /// Provides an axis that displays seperators based upon configured windows
    /// </summary>
    public class WindowAxisCore : AxisCore
    {
        private IAxisWindow _selectedWindow;

        private IAxisWindow SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                if (Equals(_selectedWindow, value)) return;
                _selectedWindow = value;               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IAxisWindow> Windows { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public WindowAxisCore(IAxisView view) : base(view)
        {            
        }

        /// <summary>
        /// 
        /// </summary>
        internal override CoreMargin PrepareChart(AxisOrientation source, ChartCore chart)
        {
            //if (TopLimit <= 0 && BotLimit < 0) throw new Exception("DateAxis does not support negative values");

            if (!(Math.Abs(TopLimit - BotLimit) > S * .01) || !ShowLabels) return new CoreMargin();

            // Calculate the separators and the resolution
            CalculateSeparator(chart, source);

            var currentMargin = new CoreMargin();
            var tolerance = S / 10;

            InitializeGarbageCollector();

            // Determine which magnitude and unit to use
            var m = (!double.IsNaN(View.Unit) ? View.Unit : Magnitude);
            var u = (!double.IsNaN(View.Unit) ? View.Unit : 1);

            // Loop trough all the 
            // I use ceiling, because possibly the X was out of range
            for (var x = Math.Floor(BotLimit); x <= TopLimit - (EvaluatesUnitWidth ? u : 0); x += 1)
            {
                LastSeparator = x;
                
                // Filter for actual separators
                if (SelectedWindow.IsSeparator(x))
                {
                    DrawSeparator(x, tolerance, currentMargin, source);
                }
            }

            return currentMargin;
        }

        internal override void CalculateSeparator(ChartCore chart, AxisOrientation source)
        {
            if (!double.IsNaN(Separator.Step)) throw new Exception("Step should be NaN for WindowAxis separators");
            if (Windows.Count == 0) throw new Exception("No windows have been configured");

            // Find the seperator resolution represented by the first available window
            double supportedSeparatorCount = 0;
            var proposedWindow = Windows
                .FirstOrDefault(w =>
                {
                    // Calculate the number of separators we support
                    supportedSeparatorCount = Math.Round(chart.ControlSize.Width / (w.MinimumSeparatorWidth * CleanFactor), 0);

                    // Calculate the number of required separators
                    var requiredSeparatorCount = 0;

                    for (var x = Math.Floor(BotLimit); x <= TopLimit; x += 1)
                    {
                        if (w.IsSeparator(x))
                        {
                            requiredSeparatorCount++;
                        }
                    }

                    return supportedSeparatorCount >= requiredSeparatorCount;
                });

            if (proposedWindow == null)
            {
                // Apparently no window can be used to display the X axis. Show no labels as fallback.
                proposedWindow = AxisWindows.EmptyWindow;
                supportedSeparatorCount = Math.Round(chart.ControlSize.Width / (proposedWindow.MinimumSeparatorWidth * CleanFactor), 0);
            }

            // Force the step of 1, as our preparechart will filter the X asis for valid separators, and will skip a few
            S = 1;

            Magnitude = Math.Pow(10, Math.Floor(Math.Log(supportedSeparatorCount) / Math.Log(10)));
            SelectedWindow = proposedWindow;
        }

        private void DrawSeparator(double x, double tolerance, CoreMargin currentMargin, AxisOrientation source)
        {
            SeparatorElementCore elementCore;

            var key = Math.Round(x / tolerance) * tolerance;

            if (!Cache.TryGetValue(key, out elementCore))
            {
                elementCore = new DateSeparatorElementCore { IsNew = true };
                Cache[key] = elementCore;
            }
            else
            {
                elementCore.IsNew = false;
            }

            // Determine whether this separator is a header now
            ((DateSeparatorElementCore)elementCore).IsHeader = SelectedWindow.IsHeader(x);

            View.RenderSeparator(elementCore, Chart);

            elementCore.Key = key;
            elementCore.Value = x;
            elementCore.GarbageCollectorIndex = GarbageCollectorIndex;

            var labelsMargin = elementCore.View.UpdateLabel(SelectedWindow.FormatAxisLabel(x), this, source);

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
                elementCore.State = SeparationState.InitialAdd;
                return;
            }

            elementCore.State = SeparationState.Keep;
        }
    }
}