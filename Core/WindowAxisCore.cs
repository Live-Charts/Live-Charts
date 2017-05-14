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
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;

namespace LiveCharts
{
    /// <summary>
    /// Provides an axis that displays separators based upon configured windows
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
                ((IWindowAxisView)View).SetSelectedWindow(value);
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
            if (!(Math.Abs(TopLimit - BotLimit) > S * .01) || !ShowLabels) return new CoreMargin();

            var currentMargin = new CoreMargin();
            var tolerance = S / 10;

            InitializeGarbageCollector();

            // Determine which magnitude and unit to use
            var m = (!double.IsNaN(View.Unit) ? View.Unit : Magnitude);
            var u = (!double.IsNaN(View.Unit) ? View.Unit : 1);

            // Calculate the separator indices
            var indices = CalculateSeparatorIndices(chart, source, u);

            // Draw the separators
            foreach (var index in indices)
            {
                DrawSeparator(index, tolerance, currentMargin, source);
            }

            return currentMargin;
        }

        internal IEnumerable<double> CalculateSeparatorIndices(ChartCore chart, AxisOrientation source, double unit)
        {
            if (!double.IsNaN(Separator.Step)) throw new Exception("Step should be NaN for WindowAxis separators");
            if (Windows == null) return Enumerable.Empty<double>();

            // Holder for the calculated separator indices and the proposed window
            var supportedSeparatorCount = 0;
            var separatorIndices = new List<double>();
            IAxisWindow proposedWindow = AxisWindows.EmptyWindow;

            // Build a range of possible separator indices
            var rangeIndices = Enumerable.Range((int)Math.Floor(BotLimit), (int)Math.Floor(TopLimit - (EvaluatesUnitWidth ? unit : 0) - BotLimit)).Select(i => (double)i).ToList();

            // Make sure we have at least 2 separators to show
            if (Windows != null && rangeIndices.Count > 1)
            {
                foreach (var window in Windows)
                {
                    IEnumerable<double> proposedSeparatorIndices;

                    // Calculate the number of supported separators.
                    supportedSeparatorCount = (int)Math.Floor(chart.ControlSize.Width / (window.MinimumSeparatorWidth * CleanFactor));

                    // Try go get separators. Continue if the window invalidated.
                    if (!window.TryGetSeparatorIndices(rangeIndices, supportedSeparatorCount, out proposedSeparatorIndices)) continue;

                    // Double check whether the window exceeded the maximum separator count.
                    // It might be it does not respect the supportedSeparatorCount parameter.
                    separatorIndices = proposedSeparatorIndices.ToList();
                    if (supportedSeparatorCount < separatorIndices.Count) continue;
                    
                    // Pick this window. It is the first who passed both validations and our best candidate
                    proposedWindow = window;
                    break;
                }
            }

            if (proposedWindow == null)
            {
                // All variables are still set to defaults
            }

            // Force the step of 1, as our prepare chart will filter the X axis for valid separators, and will skip a few
            S = 1;

            Magnitude = Math.Pow(10, Math.Floor(Math.Log(supportedSeparatorCount) / Math.Log(10)));
            SelectedWindow = proposedWindow;

            return separatorIndices;
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