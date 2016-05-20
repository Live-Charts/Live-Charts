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
using LiveCharts.Defaults;

namespace LiveCharts.SeriesAlgorithms
{
    public class ColumnAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        public ColumnAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Horizontal;
        }

        public override void Update()
        {
            var castedSeries = (IColumnSerieView) View;

            const double padding = 5;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisTags.X, Chart, View.ScalesXAt) - padding;
            var typeSeries = Chart.View.Series.OfType<IColumnSerieView>().ToList();

            var singleColWidth = totalSpace/typeSeries.Count;

            double exceed = 0;

            var seriesPosition = typeSeries.IndexOf(castedSeries);

            if (singleColWidth > castedSeries.MaxColumnWidth)
            {
                exceed = (singleColWidth - castedSeries.MaxColumnWidth)*typeSeries.Count/2;
                singleColWidth = castedSeries.MaxColumnWidth;
            }

            var relativeLeft = padding + exceed + singleColWidth*(seriesPosition);

            var startAt = CurrentYAxis.MinLimit >= 0 && CurrentYAxis.MaxLimit > 0   //both positive
                ? CurrentYAxis.MinLimit                                             //then use axisYMin
                : (CurrentYAxis.MinLimit < 0 && CurrentYAxis.MaxLimit <= 0          //both negative
                    ? CurrentYAxis.MaxLimit                                         //then use axisYMax
                    : 0);                                                           //if mixed then use 0

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisTags.Y, Chart, View.ScalesYAt);

            foreach (var chartPoint in View.Values.Points)
            {
                var reference =
                    ChartFunctions.ToDrawMargin(chartPoint, View.ScalesXAt, View.ScalesYAt, Chart);

                chartPoint.View = View.GetPointView(chartPoint.View, chartPoint,
                    View.DataLabels ? View.LabelPoint(chartPoint) : null);

                chartPoint.SeriesView = View;

                var rectangleView = (IRectanglePointView) chartPoint.View;

                var h = Math.Abs(reference.Y - zero);
                var t = reference.Y < zero
                    ? reference.Y
                    : zero;

                rectangleView.Data.Height = h;
                rectangleView.Data.Top = t;

                rectangleView.Data.Left = reference.X + relativeLeft;
                rectangleView.Data.Width = singleColWidth - padding;

                rectangleView.ZeroReference = zero;

                chartPoint.ChartLocation = new CorePoint(rectangleView.Data.Left + singleColWidth/2 - padding/2,
                    t);

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }

        double ICartesianSeries.GetMinX(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxX(AxisCore axis)
        {
            return AxisLimits.UnitRight(axis);
        }

        double ICartesianSeries.GetMinY(AxisCore axis)
        {
            var f = AxisLimits.SeparatorMin(axis);
            return CurrentYAxis.MinLimit >= 0 && CurrentYAxis.MaxLimit > 0
                ? (f >= 0 ? f : 0)
                : f;
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            var f = AxisLimits.SeparatorMax(axis);
            return CurrentYAxis.MinLimit < 0 && CurrentYAxis.MaxLimit <= 0
                ? (f >= 0 ? f : 0)
                : f;
        }
    }
}
