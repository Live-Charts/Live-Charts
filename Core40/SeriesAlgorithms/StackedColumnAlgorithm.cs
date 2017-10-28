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
using LiveCharts.Defaults;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using System.Linq;

namespace LiveCharts.SeriesAlgorithms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.SeriesAlgorithm" />
    /// <seealso cref="LiveCharts.Definitions.Series.ICartesianSeries" />
    public class StackedColumnAlgorithm : SeriesAlgorithm , ICartesianSeries
    {
        private readonly IStackModelableSeriesView _stackModelable;
        /// <summary>
        /// Initializes a new instance of the <see cref="StackedColumnAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public StackedColumnAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Horizontal;
            _stackModelable = (IStackModelableSeriesView) view;
            PreferredSelectionMode = TooltipSelectionMode.SharedXValues;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            var castedSeries = (IStackedColumnSeriesView) View;

            var padding = castedSeries.ColumnPadding;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisOrientation.X, Chart, View.ScalesXAt) - padding;
            var groups = Chart.View.ActualSeries.Select(s => (s as IGroupedStackedSeriesView)?.Grouping).Distinct().ToList();
            var singleColWidth = totalSpace / groups.Count();

            double exceed = 0;
            var seriesPosition = groups.IndexOf(castedSeries.Grouping);

            if (singleColWidth > castedSeries.MaxColumnWidth)
            {
                exceed = (singleColWidth - castedSeries.MaxColumnWidth) * groups.Count() / 2;
                singleColWidth = castedSeries.MaxColumnWidth;
            }

            var relativeLeft = padding + exceed + singleColWidth * (seriesPosition);

            var startAt = CurrentYAxis.FirstSeparator >= 0 && CurrentYAxis.LastSeparator > 0
                ? CurrentYAxis.FirstSeparator
                : (CurrentYAxis.FirstSeparator < 0 && CurrentYAxis.LastSeparator <= 0
                ? CurrentYAxis.LastSeparator
                    : 0);

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisOrientation.Y, Chart, View.ScalesYAt);

            foreach (var chartPoint in View.ActualValues.GetPoints(View))
            {
                var x = ChartFunctions.ToDrawMargin(chartPoint.X, AxisOrientation.X, Chart, View.ScalesXAt);
                var from = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.From, AxisOrientation.Y, Chart, View.ScalesYAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.From/chartPoint.Sum, AxisOrientation.Y, Chart, View.ScalesYAt);
                var to = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.To, AxisOrientation.Y, Chart, View.ScalesYAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.To/chartPoint.Sum, AxisOrientation.Y, Chart, View.ScalesYAt);

                if (double.IsNaN(from)) from = 0;
                if (double.IsNaN(to)) to = 0;

                chartPoint.View = View.GetPointView(chartPoint,
                    View.DataLabels
                        ? (chartPoint.Participation > 0.05
                            ? View.GetLabelPointFormatter()(chartPoint)
                            : string.Empty)
                        : null);

                chartPoint.SeriesView = View;

                var rectangleView = (IRectanglePointView) chartPoint.View;

                var h = Math.Abs(to - zero) - Math.Abs(from - zero);
                var t = to < zero
                    ? to
                    : from;

                rectangleView.Data.Height = h;
                rectangleView.Data.Top = t;

                rectangleView.Data.Left = x + relativeLeft;
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
            return double.MaxValue;
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            return double.MinValue;
        }
    }
}
