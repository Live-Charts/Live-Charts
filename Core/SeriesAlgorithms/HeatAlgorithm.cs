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

using System.Xml;
using LiveCharts.Defaults;

namespace LiveCharts.SeriesAlgorithms
{
    public class HeatAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        public HeatAlgorithm(ISeriesView view) : base(view)
        {
            PreferredSelectionMode = TooltipSelectionMode.OnlySender;
        }

        public override void Update()
        {
            var heatSeries = (IHeatSeries) View;

            var uw = new CorePoint(
                0*ChartFunctions.GetUnitWidth(AxisTags.X, Chart, View.ScalesXAt)/2,
                - ChartFunctions.GetUnitWidth(AxisTags.Y, Chart, View.ScalesYAt));

            var wd = CurrentXAxis.MaxLimit - CurrentXAxis.MinLimit == 0
                ? double.MaxValue
                : CurrentXAxis.MaxLimit - CurrentXAxis.MinLimit;
            var hd = CurrentYAxis.MaxLimit - CurrentYAxis.MinLimit == 0
                ? double.MaxValue
                : CurrentYAxis.MaxLimit - CurrentYAxis.MinLimit;
            var w = Chart.DrawMargin.Width/wd;
            var h = Chart.DrawMargin.Height/hd;

            foreach (var chartPoint in View.ActualValues.Points)
            {
                chartPoint.ChartLocation = ChartFunctions.ToDrawMargin(
                    chartPoint, View.ScalesXAt, View.ScalesYAt, Chart) + uw;

                chartPoint.SeriesView = View;

                chartPoint.View = View.GetPointView(chartPoint.View, chartPoint,
                    View.DataLabels ? View.LabelPoint(chartPoint) : null);

                var heatView = (IHeatPointView) chartPoint.View;

                heatView.ColorComponents = ColorInterpolation(
                    heatSeries.ColdComponents, heatSeries.HotComponents, chartPoint.Weight);

                heatView.Width = w;
                heatView.Height = h;

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }

        public double GetMinX(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        public double GetMaxX(AxisCore axis)
        {
            return AxisLimits.StretchMax(axis) + 1;
        }

        public double GetMinY(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        public double GetMaxY(AxisCore axis)
        {
            return AxisLimits.StretchMax(axis) + 1;
        }

        private CoreColor ColorInterpolation(CoreColor from, CoreColor to, double weight)
        {
            return new CoreColor(
                LinearInterpolation(from.A, to.A, weight),
                LinearInterpolation(from.R, from.R, weight),
                LinearInterpolation(from.G, to.G, weight),
                LinearInterpolation(from.B, to.B, weight));
        }

        private byte LinearInterpolation(byte from, byte to, double value)
        {
            var p1 = new CorePoint(Chart.Value3CoreLimit.Min, from);
            var p2 = new CorePoint(Chart.Value3CoreLimit.Max, to);

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);

            return (byte) (m*(value - p1.X) + p1.Y);
        }
    }
}
