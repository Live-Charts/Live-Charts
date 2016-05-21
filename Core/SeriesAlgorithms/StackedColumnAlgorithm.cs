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
    public class StackedColumnAlgorithm : SeriesAlgorithm , ICartesianSeries
    {
        private readonly IStackModelableSeriesView _stackModelable;
        public StackedColumnAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Horizontal;
            _stackModelable = (IStackModelableSeriesView) view;
        }

        public override void Update()
        {
            var castedSeries = (IStackedColumnSeriesViewView) View;

            const double padding = 5;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisTags.X, Chart, View.ScalesXAt) - padding;
            var singleColWidth = totalSpace;

            double exceed = 0;

            if (singleColWidth > castedSeries.MaxColumnWidth)
            {
                exceed = (singleColWidth - castedSeries.MaxColumnWidth)/2;
                singleColWidth = castedSeries.MaxColumnWidth;
            }

            var relativeLeft = padding + exceed;

            var startAt = CurrentYAxis.MinLimit >= 0 && CurrentYAxis.MaxLimit > 0
                ? CurrentYAxis.MinLimit                                            
                : (CurrentYAxis.MinLimit < 0 && CurrentYAxis.MaxLimit <= 0          
                ? CurrentYAxis.MaxLimit                                         
                    : 0);                                                         

            var zero = ChartFunctions.ToDrawMargin(startAt, AxisTags.Y, Chart, View.ScalesYAt);

            foreach (var chartPoint in View.Values.Points)
            {
                var x = ChartFunctions.ToDrawMargin(chartPoint.X, AxisTags.X, Chart, View.ScalesXAt);
                var from = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.From, AxisTags.Y, Chart, View.ScalesYAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.From/chartPoint.Sum, AxisTags.Y, Chart, View.ScalesYAt);
                var to = _stackModelable.StackMode == StackMode.Values
                    ? ChartFunctions.ToDrawMargin(chartPoint.To, AxisTags.Y, Chart, View.ScalesYAt)
                    : ChartFunctions.ToDrawMargin(chartPoint.To/chartPoint.Sum, AxisTags.Y, Chart, View.ScalesYAt);

                if (double.IsNaN(from)) from = 0;
                if (double.IsNaN(to)) to = 0;

                chartPoint.View = View.GetPointView(chartPoint.View, chartPoint,
                    View.DataLabels
                        ? (chartPoint.Participation > 0.05 ? View.LabelPoint(chartPoint) : string.Empty)
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
