using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;

namespace LiveCharts
{
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
        public List<AxisWindow> Windows { get; set; }

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
            if (SelectedWindow != null)
            {
                for (var x = Math.Floor(BotLimit); x <= TopLimit - (EvaluatesUnitWidth ? u : 0); x += 1)
                {
                    LastSeparator = x;

                    // Filter for actual separators
                    if (SelectedWindow.IsSeparator(x))
                    {
                        DrawSeparator(x, tolerance, currentMargin, source);
                    }
                }
            }

            return currentMargin;
        }

        internal override void CalculateSeparator(ChartCore chart, AxisOrientation source)
        {
            if (!double.IsNaN(Separator.Step)) throw new Exception("Step should be NaN for WindowAxis separators");
            if (Windows == null) return;

            // Find the seperator resolution represented by the first available window
            double supportedSeparatorCount = 0;

            AxisWindow proposedWindow = null;
            if (Windows != null)
            {
                proposedWindow = Windows.FirstOrDefault(w =>
                {
                // Calculate the number of separators we support
                supportedSeparatorCount = Math.Round(chart.ControlSize.Width / (w.MinimumSeparatorWidth * CleanFactor),
                        0);

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
            }

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