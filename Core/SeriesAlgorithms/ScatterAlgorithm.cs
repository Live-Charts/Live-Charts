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

namespace LiveCharts.SeriesAlgorithms
{
    public class ScatterAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        public ScatterAlgorithm(ISeriesView view) : base(view)
        {
            XAxisMode = AxisLimitsMode.Stretch;
            YAxisMode = AxisLimitsMode.Stretch;
        }

        public AxisLimitsMode XAxisMode { get; set; }
        public AxisLimitsMode YAxisMode { get; set; }

        public override void Update()
        {
            var fx = CurrentXAxis.GetFormatter();
            var fy = CurrentYAxis.GetFormatter();

            var buubleSeries = (IBubbleSeries) View;

            var p1 = new LvcPoint();
            var p2 = new LvcPoint();

            p1.X = Chart.Value3Limit.Max;
            p1.Y = buubleSeries.MaxBubbleDiameter;

            p2.X = Chart.Value3Limit.Min;
            p2.Y = buubleSeries.MinBubbleDiameter;

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);

            var uw = new LvcPoint(
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

                chartPoint.View = View.GetPointView(chartPoint.View,
                    View.DataLabels ? fx(chartPoint.X) + ", " + fy(chartPoint.Y) : null);

                var bubbleView = (IDiameterData) chartPoint.View;

                bubbleView.Diameter = m*(chartPoint.Weight - p1.X) + p1.Y;

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }
    }
}
