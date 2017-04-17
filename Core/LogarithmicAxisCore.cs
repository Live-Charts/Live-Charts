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

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.AxisCore" />
    public class LogarithmicAxisCore : AxisCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxisCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public LogarithmicAxisCore(IAxisView view) : base(view)
        {
            CleanFactor = 1.5;
        }

        internal override CoreMargin PrepareChart(AxisOrientation source, ChartCore chart)
        {
            if (!(Math.Abs(TopLimit - BotLimit) > S * .01) || !ShowLabels) return new CoreMargin();

            CalculateSeparator(chart, source);

            var f = GetFormatter();

            var currentMargin = new CoreMargin();
            if (S < 1) S = 1;
            var tolerance = S / 10;

            InitializeGarbageCollector();

            var bl = Math.Ceiling(BotLimit / Magnitude) * Magnitude;
            var @base = ((ILogarithmicAxisView) View).Base;

            for (var i = bl; i <= TopLimit - (EvaluatesUnitWidth ? 1 : 0); i += S)
            {
                var minTolerance = tolerance/10;
                if (Math.Abs(i - bl) > tolerance)
                {
                    var step = Math.Pow(@base, i - 1);
                    for (var j = Math.Pow(@base, i - 1) + step; 
                        j < Math.Pow(@base, i); 
                        j += step)
                    {
                        SeparatorElementCore minorAsc;
                        var scaledJ = Math.Log(j, @base);

                        var minorKey = Math.Round(scaledJ / minTolerance) * minTolerance;
                        if (!Cache.TryGetValue(minorKey, out minorAsc))
                        {
                            minorAsc = new SeparatorElementCore { IsNew = true };
                            Cache[minorKey] = minorAsc;
                        }
                        else
                        {
                            minorAsc.IsNew = false;
                        }

                        View.RenderSeparator(minorAsc, Chart);

                        minorAsc.Key = minorKey;
                        minorAsc.Value = scaledJ;
                        minorAsc.GarbageCollectorIndex = GarbageCollectorIndex;

                        minorAsc.View.UpdateLabel(string.Empty, this, source);

                        if (LastAxisMax == null)
                        {
                            minorAsc.State = SeparationState.InitialAdd;
                            continue;
                        }

                        minorAsc.State = SeparationState.Keep;
                    }
                }

                SeparatorElementCore asc;

                var key = Math.Round(i / tolerance) * tolerance;
                if (!Cache.TryGetValue(key, out asc))
                {
                    asc = new SeparatorElementCore { IsNew = true };
                    Cache[key] = asc;
                }
                else
                {
                    asc.IsNew = false;
                }

                View.RenderSeparator(asc, Chart);

                asc.Key = key;
                asc.Value = i;
                asc.GarbageCollectorIndex = GarbageCollectorIndex;

                var labelsMargin = asc.View.UpdateLabel(f(i), this, source);

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
    }
}