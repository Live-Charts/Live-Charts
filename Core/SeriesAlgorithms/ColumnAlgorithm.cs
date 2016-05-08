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
    public class ColumnAlgorithm : SeriesAlgorithm , ICartesianSeries
    {
        public ColumnAlgorithm(ISeriesView view) : base(view)
        {
            XAxisMode = AxisLimitsMode.Stretch;
            YAxisMode = AxisLimitsMode.Stretch;
        }

        public AxisLimitsMode XAxisMode { get; set; }
        public AxisLimitsMode YAxisMode { get; set; }

        public override void Update()
        {
            var fy = CurrentYAxis.GetFormatter();

            var columnSeries = (IColumnSeries) View;

            const double padding = 2.5;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisTags.X, Chart, View.ScalesXAt);
            var columnSeriesCount = Chart.View.Series.OfType<IColumnSeries>().Count();

            var singleColWidth = totalSpace/columnSeriesCount;

            double exceed = 0;

            if (singleColWidth > columnSeries.MaxColumnWidth)
            {
                exceed = singleColWidth - columnSeries.MaxColumnWidth;
                singleColWidth -= exceed;
            }

            var relativeLeft = padding + exceed/2 + singleColWidth*(Chart.View.Series.IndexOf(View));

            var startAt = CurrentYAxis.MinLimit >= 0 && CurrentYAxis.MaxLimit > 0   //both positive
                ? CurrentYAxis.MinLimit                                             //then use axisYMin
                : (CurrentYAxis.MinLimit <= 0 && CurrentYAxis.MaxLimit < 0          //both negative
                    ? CurrentYAxis.MaxLimit                                         //then use axisYMax
                    : 0);                                                           //if mixed then use 0

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisTags.Y, Chart, View.ScalesYAt);

            foreach (var chartPoint in View.Values.Points)
            {
                chartPoint.ChartLocation = ChartFunctions.ToDrawMargin(chartPoint, View.ScalesXAt, View.ScalesYAt, Chart);

                chartPoint.View = View.GetPointView(chartPoint.View,
                    View.DataLabels ? fy(chartPoint.Y) : null);

                var rectangleView = (IRectangleData) chartPoint.View;

                rectangleView.Data.Height = chartPoint.ChartLocation.Y > 0 ? chartPoint.ChartLocation.Y : 0;
                rectangleView.Data.Top = zero - rectangleView.Data.Height;
                rectangleView.Data.Left = chartPoint.ChartLocation.X + relativeLeft;
                rectangleView.Data.Width = relativeLeft - padding > 0 ? relativeLeft - padding : 0;

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }
    }
}
