//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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
using System.Globalization;
using System.Linq;
using LiveCharts.Charts;

namespace LiveCharts
{
    public class AxisCore
    {
        public AxisCore(IAxisView view)
        {
            View = view;
            CleanFactor = 3;
            Cache = new Dictionary<double, SeparatorElementCore>();
        }

        public ChartCore Chart { get; set; }
        public IAxisView View { get; set; }
        public IList<string> Labels { get; set; }
        public Func<double, string> LabelFormatter { get; set; }
        public double StrokeThickness { get; set; }
        public bool ShowLabels { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; }
        public string Title { get; set; }
        public bool DisableAnimations { get; set; }
        public AxisPosition Position { get; set; }
        public bool IsMerged { get; set; }

        public SeparatorConfigurationCore Separator { get; set; }

        internal double MaxLimit { get; set; }
        internal double MinLimit { get; set; }
        internal double S { get; set; }
        internal double Magnitude { get; set; }
        internal bool EvaluatesUnitWidth { get; set; }
        internal int CleanFactor { get; set; }
        internal Dictionary<double, SeparatorElementCore> Cache { get; set; }
        internal double? LastAxisMax { get; set; }
        internal double? LastAxisMin { get; set; }
        internal LvcRectangle LastPlotArea { get; set; }

        internal void CalculateSeparator(ChartCore chart, AxisTags source)
        {
            if (Separator.Step != null)
            {
                S = Separator.Step ?? 1;
                return;
            }

            var range = MaxLimit - MinLimit;
            range = range <= 0 ? 1 : range;

            //ToDO: Improve this according to current labels!
            var separations = source == AxisTags.Y
                ? Math.Round(chart.DrawMargin.Height / ((12) * CleanFactor), 0) // at least 3 font 12 labels per separator.
                : Math.Round(chart.DrawMargin.Width / (50 * CleanFactor), 0);   // at least 150 pixels per separator.

            separations = separations < 2 ? 2 : separations;

            var minimum = range / separations;
            Magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));
            var residual = minimum / Magnitude;
            double tick;
            if (residual > 5)
                tick = 10*Magnitude;
            else if (residual > 2)
                tick = 5 * Magnitude;
            else if (residual > 1)
                tick = 2 * Magnitude;
            else
                tick = Magnitude;

            S = tick;

            if (Labels != null) S = S < 1 ? 1 : S; //Todo: Check this , this should be wrong
        }

        public double FromPreviousAxisState(double value, AxisTags source, ChartCore chart)
        {
            if (LastAxisMax == null) return 0;

            var p1 = new LvcPoint();
            var p2 = new LvcPoint();

            if (source == AxisTags.Y)
            {
                p1.X = LastAxisMax ?? 0;
                p1.Y = LastPlotArea.Top;

                p2.X = LastAxisMin ?? 0;
                p2.Y = LastPlotArea.Top + LastPlotArea.Height;
            }
            else
            {
                p1.X = LastAxisMax ?? 0;
                p1.Y = LastPlotArea.Width + LastPlotArea.Left;

                p2.X = LastAxisMin ?? 0;
                p2.Y = LastPlotArea.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        internal LvcSize PrepareChart(AxisTags source, ChartCore chart)
        {
            if (!(Math.Abs(MaxLimit - MinLimit) > S * .01) || !ShowLabels) return new LvcSize();
            if (chart.DrawMargin.Width < 5 || chart.DrawMargin.Height < 5) return new LvcSize();

            CalculateSeparator(chart, source);

            var f = GetFormatter();

            var biggest = new LvcSize(0, 0);
            var tolerance = S / 10;

            var uwc = 0;

            //Instead each axis will have a unitary width
            //if (chart.HasUnitaryPoints)
            //    if (source == AxisTags.X && !chart.Invert) uwc = 1;

            for (var i = MinLimit; i <= MaxLimit - uwc; i += S)
            {
                SeparatorElementCore asc;

                var key = Math.Round(i / tolerance) * tolerance;
                if (!Cache.TryGetValue(key, out asc))
                {
                    asc = new SeparatorElementCore {IsNew = true};
                    Cache[key] = asc;
                }
                else
                {
                    asc.IsNew = false;
                }

                View.RenderSeparator(asc, Chart);

                asc.Key = key;
                asc.Value = i;
                asc.IsActive = true;

                var labelsSize = asc.View.UpdateLabel(f(i));

                biggest.Width = labelsSize.Width > biggest.Width
                    ? labelsSize.Width
                    : biggest.Width;
                biggest.Height = labelsSize.Height > biggest.Height
                    ? labelsSize.Height
                    : biggest.Height;

                if (LastAxisMax == null)
                {
                    asc.State = SeparationState.InitialAdd;
                    continue;
                }

                asc.State = SeparationState.Keep;
            }
#if DEBUG
            Debug.WriteLine("Axis.Separations: " + Cache.Count);
#endif
            return biggest;
        }

        internal void UpdateSeparators(AxisTags source, ChartCore chart, int axisPosition)
        {
            foreach (var element in Cache.Values.ToArray())
            {
                if (!element.IsActive)
                {
                    element.State = SeparationState.Remove;
                    Cache.Remove(element.Key);
                }
                element.View.UpdateLine(source, chart, axisPosition, this);
                element.IsActive = false;
            }

            LastAxisMax = MaxLimit;
            LastAxisMin = MinLimit;
            LastPlotArea = new LvcRectangle(chart.DrawMargin.Left, chart.DrawMargin.Top,
                chart.DrawMargin.Width, chart.DrawMargin.Height);
        }

        internal Func<double, string> GetFormatter()
        {
            return x => Labels == null
                ? (LabelFormatter == null
                    ? x.ToString(CultureInfo.InvariantCulture)
                    : LabelFormatter(x))
                : (Labels.Count > x && x >= 0
                    ? Labels[(int)x]
                    : "");
        }
    }
}
