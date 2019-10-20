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
    public class StackedRowAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        private readonly IStackModelableSeriesView _stackModelable;
        /// <summary>
        /// Initializes a new instance of the <see cref="StackedRowAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public StackedRowAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Vertical;
            _stackModelable = (IStackModelableSeriesView) view;
            PreferredSelectionMode = TooltipSelectionMode.SharedYValues;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            var castedSeries = (IStackedRowSeriesView) View;

            var padding = castedSeries.RowPadding;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisOrientation.Y, Chart, View.ScalesYAt) - padding;
            var groups = Chart.View.ActualSeries.Select(s => (s as IStackedColumnSeriesView)?.Grouping).Distinct().ToList();
            var singleColHeigth = totalSpace / groups.Count();

            double exceed = 0;
            var seriesPosition = groups.IndexOf(castedSeries.Grouping);

            if (singleColHeigth > castedSeries.MaxRowHeight)
            {
                exceed = (singleColHeigth - castedSeries.MaxRowHeight) * groups.Count() / 2;
                singleColHeigth = castedSeries.MaxRowHeight;
            }

            var relativeTop = padding + exceed + singleColHeigth * (seriesPosition);

            var startAt = CurrentXAxis.FirstSeparator >= 0 && CurrentXAxis.LastSeparator > 0
                ? CurrentXAxis.FirstSeparator
                : (CurrentXAxis.FirstSeparator < 0 && CurrentXAxis.LastSeparator <= 0
                    ? CurrentXAxis.LastSeparator
                    : 0);

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisOrientation.X, Chart, View.ScalesXAt);

            foreach (var chartPoint in View.ActualValues.GetPoints(View))
            {
                var y = ChartFunctions.ToDrawMargin(chartPoint.Y, AxisOrientation.Y, Chart, View.ScalesYAt) - ChartFunctions.GetUnitWidth(AxisOrientation.Y, Chart, View.ScalesYAt);
                var from = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.From, AxisOrientation.X, Chart, View.ScalesXAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.From/chartPoint.Sum, AxisOrientation.X, Chart, View.ScalesXAt);
                var to = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.To, AxisOrientation.X, Chart, View.ScalesXAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.To/chartPoint.Sum, AxisOrientation.X, Chart, View.ScalesXAt);

                chartPoint.View = View.GetPointView(chartPoint,
                    View.DataLabels
                        ? (chartPoint.Participation > 0.05
                            ? View.GetLabelPointFormatter()(chartPoint)
                            : string.Empty)
                        : null);

                chartPoint.SeriesView = View;

                var rectangleView = (IRectanglePointView) chartPoint.View;

                var w = Math.Abs(to - zero) - Math.Abs(from - zero);
                var l = to < zero
                    ? to
                    : from;

                rectangleView.Data.Height = singleColHeigth - padding;
                rectangleView.Data.Top = y + relativeTop;

                rectangleView.Data.Left = l;
                rectangleView.Data.Width = w;

                rectangleView.ZeroReference = zero;

                chartPoint.ChartLocation = new CorePoint(rectangleView.Data.Left + rectangleView.Data.Width,
                    rectangleView.Data.Top);

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }

        double ICartesianSeries.GetMinX(AxisCore axis)
        {
            return double.MaxValue;
        }

        double ICartesianSeries.GetMaxX(AxisCore axis)
        {
            return double.MinValue;
        }

        double ICartesianSeries.GetMinY(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            return AxisLimits.UnitRight(axis);
        }
    }
}
