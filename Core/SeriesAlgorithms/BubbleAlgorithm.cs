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

using LiveCharts.Defaults;

namespace LiveCharts.SeriesAlgorithms
{
    public class BubbleAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        public BubbleAlgorithm(ISeriesView view) : base(view)
        {
        }

        public override void Update()
        {
            var buubleSeries = (IBubbleSeriesView) View;

            var p1 = new CorePoint();
            var p2 = new CorePoint();

            p1.X = Chart.Value3CoreLimit.Max;
            p1.Y = buubleSeries.MaxBubbleDiameter;

            p2.X = Chart.Value3CoreLimit.Min;
            p2.Y = buubleSeries.MinBubbleDiameter;

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);

            var uw = new CorePoint(
                    CurrentXAxis.EvaluatesUnitWidth
                        ? ChartFunctions.GetUnitWidth(AxisTags.X, Chart, View.ScalesXAt) / 2
                        : 0,
                    CurrentYAxis.EvaluatesUnitWidth
                        ? ChartFunctions.GetUnitWidth(AxisTags.Y, Chart, View.ScalesYAt) / 2
                        : 0);

            foreach (var chartPoint in View.Values.Points)
            {
                chartPoint.ChartLocation = ChartFunctions.ToDrawMargin(
                    chartPoint, View.ScalesXAt, View.ScalesYAt, Chart) + uw;

                chartPoint.SeriesView = View;

                chartPoint.View = View.GetPointView(chartPoint.View, chartPoint,
                    View.DataLabels ? View.LabelPoint(chartPoint) : null);

                var bubbleView = (IBubblePointView) chartPoint.View;

                bubbleView.Diameter = m*(chartPoint.Weight - p1.X) + p1.Y;

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }

        double ICartesianSeries.GetMinX(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxX(AxisCore axis)
        {
            return AxisLimits.StretchMax(axis);
        }

        double ICartesianSeries.GetMinY(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            return AxisLimits.StretchMax(axis);
        }
    }
}
