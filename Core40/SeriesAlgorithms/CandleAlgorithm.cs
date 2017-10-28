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
    public class CandleAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CandleAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public CandleAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Horizontal;
            PreferredSelectionMode = TooltipSelectionMode.SharedXValues;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            var castedSeries = (IFinancialSeriesView) View;
            
            const double padding = 1.2;

            var totalSpace = ChartFunctions.GetUnitWidth(AxisOrientation.X, Chart, View.ScalesXAt) - padding;

            double exceed = 0;
            double candleWidth;

            if (totalSpace > castedSeries.MaxColumnWidth)
            {
                exceed = totalSpace - castedSeries.MaxColumnWidth;
                candleWidth = castedSeries.MaxColumnWidth;
            }
            else
            {
                candleWidth = totalSpace;
            }

            ChartPoint previousDrawn = null;

            foreach (var chartPoint in View.ActualValues.GetPoints(View))
            {
                var x = ChartFunctions.ToDrawMargin(chartPoint.X, AxisOrientation.X, Chart, View.ScalesXAt);

                chartPoint.View = View.GetPointView(chartPoint,
                    View.DataLabels ? View.GetLabelPointFormatter()(chartPoint) : null);

                chartPoint.SeriesView = View;

                var candeView = (IOhlcPointView) chartPoint.View;

                candeView.Open = ChartFunctions.ToDrawMargin(chartPoint.Open, AxisOrientation.Y, Chart, View.ScalesYAt);
                candeView.Close = ChartFunctions.ToDrawMargin(chartPoint.Close, AxisOrientation.Y, Chart, View.ScalesYAt);
                candeView.High = ChartFunctions.ToDrawMargin(chartPoint.High, AxisOrientation.Y, Chart, View.ScalesYAt);
                candeView.Low = ChartFunctions.ToDrawMargin(chartPoint.Low, AxisOrientation.Y, Chart, View.ScalesYAt);

                candeView.Width = candleWidth - padding > 0 ? candleWidth - padding : 0;
                candeView.Left = x + exceed/2 + padding;
                candeView.StartReference = (candeView.High + candeView.Low)/2;

                chartPoint.ChartLocation = new CorePoint(x + exceed/2, (candeView.High + candeView.Low)/2);

                chartPoint.View.DrawOrMove(previousDrawn, chartPoint, 0, Chart);

                previousDrawn = chartPoint;
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
            return AxisLimits.SeparatorMin(axis);
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            return AxisLimits.SeparatorMaxRounded(axis);
        }
    }
}
