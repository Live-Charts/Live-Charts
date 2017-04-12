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
using System.Linq;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.SeriesAlgorithms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.SeriesAlgorithm" />
    /// <seealso cref="LiveCharts.Definitions.Series.ICartesianSeries" />
    public class ColumnAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ColumnAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Horizontal;
            PreferredSelectionMode = TooltipSelectionMode.SharedXValues;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            var columnSeries = (IColumnSeriesView) View;

            var padding = columnSeries.ColumnPadding;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisOrientation.X, Chart, View.ScalesXAt) - padding;
            var typeSeries = Chart.View.ActualSeries.Where(x =>
            {
                if (x == View) return true;
                var icsv = x as IColumnSeriesView;
                return icsv != null && icsv.SharesPosition;
            }).ToList();

            var singleColWidth = totalSpace / typeSeries.Count;

            double exceed = 0;

            var seriesPosition = typeSeries.IndexOf(columnSeries);

            if (singleColWidth > columnSeries.MaxColumnWidth)
            {
                exceed = (singleColWidth - columnSeries.MaxColumnWidth)*typeSeries.Count/2;
                singleColWidth = columnSeries.MaxColumnWidth;
            }

            var relativeLeft = padding + exceed + singleColWidth*(seriesPosition);

            var startAt = CurrentYAxis.FirstSeparator >= 0 && CurrentYAxis.LastSeparator > 0   //both positive
                ? CurrentYAxis.FirstSeparator                                                  //then use axisYMin
                : (CurrentYAxis.FirstSeparator < 0 && CurrentYAxis.LastSeparator <= 0          //both negative
                    ? CurrentYAxis.LastSeparator                                               //then use axisYMax
                    : 0);                                                                      //if mixed then use 0

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisOrientation.Y, Chart, View.ScalesYAt);

            foreach (var chartPoint in View.ActualValues.GetPoints(View))
            {
                var reference =
                    ChartFunctions.ToDrawMargin(chartPoint, View.ScalesXAt, View.ScalesYAt, Chart);

                chartPoint.View = View.GetPointView(chartPoint,
                    View.DataLabels ? View.GetLabelPointFormatter()(chartPoint) : null);

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
            return CurrentYAxis.BotLimit >= 0 && CurrentYAxis.TopLimit > 0
                ? (f >= 0 ? f : 0)
                : f;
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            var f = AxisLimits.SeparatorMaxRounded(axis);
            return CurrentYAxis.BotLimit < 0 && CurrentYAxis.TopLimit <= 0
                ? (f >= 0 ? f : 0)
                : f;
        }
    }
}
