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
using System.Linq;

namespace LiveCharts.SeriesAlgorithms
{
    public class StackedColumnAlgorithm : SeriesAlgorithm , ICartesianSeries
    {
        public StackedColumnAlgorithm(ISeriesView view) : base(view)
        {
            XAxisMode = AxisLimitsMode.UnitWidth;
            YAxisMode = AxisLimitsMode.Separator;
            SeriesConfigurationType = SeriesConfigurationType.IndexedX;
        }

        public AxisLimitsMode XAxisMode { get; set; }
        public AxisLimitsMode YAxisMode { get; set; }

        public override void Update()
        {
            var fy = CurrentYAxis.GetFormatter();

            var columnSeries = (IStackedColumnSeries) View;

            const double padding = 5;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisTags.X, Chart, View.ScalesXAt) - padding;
            var columnSeriesCount = Chart.View.Series.OfType<IColumnSeries>().Count();

            var singleColWidth = totalSpace/columnSeriesCount;

            double exceed = 0;

            var seriesPosition = Chart.View.Series.IndexOf(View);

            if (singleColWidth > columnSeries.MaxColumnWidth)
            {
                exceed = (singleColWidth - columnSeries.MaxColumnWidth)*columnSeriesCount/2;
                singleColWidth = columnSeries.MaxColumnWidth;
            }

            var relativeLeft = padding + exceed + singleColWidth*(seriesPosition);

            var startAt = CurrentYAxis.MinLimit >= 0 && CurrentYAxis.MaxLimit > 0   //both positive
                ? CurrentYAxis.MinLimit                                             //then use axisYMin
                : (CurrentYAxis.MinLimit <= 0 && CurrentYAxis.MaxLimit < 0          //both negative
                    ? CurrentYAxis.MaxLimit                                         //then use axisYMax
                    : 0);                                                           //if mixed then use 0

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisTags.Y, Chart, View.ScalesYAt);

            foreach (var chartPoint in View.Values.Points)
            {
                var reference =
                    ChartFunctions.ToDrawMargin(chartPoint, View.ScalesXAt, View.ScalesYAt, Chart);

                chartPoint.View = View.GetPointView(chartPoint.View,
                    View.DataLabels ? fy(chartPoint.Y) : null);

                var rectangleView = (IRectangleData) chartPoint.View;

                var h = Math.Abs(reference.Y - zero);
                var t = reference.Y < zero
                    ? reference.Y
                    : zero;

                rectangleView.Data.Height = h > 0 ? h : 0;
                rectangleView.Data.Top = t;

                rectangleView.Data.Left = reference.X + relativeLeft;
                rectangleView.Data.Width = singleColWidth - padding;

                rectangleView.ZeroReference = zero;

                chartPoint.ChartLocation = new CorePoint(rectangleView.Data.Left + singleColWidth/2 - padding/2,
                    t);

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }
    }
}
