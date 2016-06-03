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
using LiveCharts.Defaults;

namespace LiveCharts.SeriesAlgorithms
{
    public class StackedRowAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        private readonly IStackModelableSeriesView _stackModelable;
        public StackedRowAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Vertical;
            _stackModelable = (IStackModelableSeriesView) view;
        }

        public override void Update()
        {
            var castedSeries = (IStackedRowSeriesViewView) View;

            const double padding = 5;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisTags.Y, Chart, View.ScalesYAt) - padding;
            var singleColHeigth = totalSpace;

            double exceed = 0;

            if (singleColHeigth > castedSeries.MaxRowHeigth)
            {
                exceed = (singleColHeigth - castedSeries.MaxRowHeigth)/2;
                singleColHeigth = castedSeries.MaxRowHeigth;
            }

            var relativeTop = padding + exceed;

            var startAt = CurrentXAxis.MinLimit >= 0 && CurrentXAxis.MaxLimit > 0
                ? CurrentXAxis.MinLimit
                : (CurrentXAxis.MinLimit < 0 && CurrentXAxis.MaxLimit <= 0
                    ? CurrentXAxis.MaxLimit
                    : 0);

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisTags.X, Chart, View.ScalesXAt);

            foreach (var chartPoint in View.Values.Points)
            {
                var y = ChartFunctions.ToDrawMargin(chartPoint.Y, AxisTags.Y, Chart, View.ScalesYAt) - ChartFunctions.GetUnitWidth(AxisTags.Y, Chart, View.ScalesYAt);
                var from = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.From, AxisTags.X, Chart, View.ScalesXAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.From/chartPoint.Sum, AxisTags.X, Chart, View.ScalesXAt);
                var to = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.To, AxisTags.X, Chart, View.ScalesXAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.To/chartPoint.Sum, AxisTags.X, Chart, View.ScalesXAt);

                chartPoint.View = View.GetPointView(chartPoint.View, chartPoint,
                    View.DataLabels
                        ? (chartPoint.Participation > 0.05 ? View.LabelPoint(chartPoint) : string.Empty)
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

                chartPoint.ChartLocation = new CorePoint(rectangleView.Data.Left + singleColHeigth/2 - padding/2, l);

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
