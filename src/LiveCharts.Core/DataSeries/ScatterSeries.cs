using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The scatter series class.
    /// </summary>
    public class ScatterSeries<TModel> 
        : CartesianSeries<TModel, Weighted2DPoint, ColumnViewModel, Point<TModel, Weighted2DPoint, ColumnViewModel>>, IScatterSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries{TModel}"/> class.
        /// </summary>
        public ScatterSeries()
        {
            LiveChartsSettings.Set<IScatterSeries>(this);
            RangeByDimension = RangeByDimension = new[]
            {
                new DataRange(), // x
                new DataRange(), // y
                new DataRange()  // w
            };
        }

        /// <inheritdoc />
        public override Point DefaultPointWidth => new Point(0, 0);

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {
            var cartesianChart = (CartesianChartModel)chart;
            var x = cartesianChart.XAxis[ScalesXAt];
            var y = cartesianChart.YAxis[ScalesYAt];
            var unitWidth = new Point(
                Math.Abs(chart.ScaleToUi(0, x) - chart.ScaleToUi(x.ActualPointWidth.X, x)),
                Math.Abs(chart.ScaleToUi(0, y) - chart.ScaleToUi(y.ActualPointWidth.Y, y)));

            //foreach (var current in Points)
            //{
            //    var p = chart.ScaleToUi(current.Coordinate, x, y);
            //    chartPoint.ChartLocation = ChartFunctions.ToDrawMargin(
            //                                   chartPoint, View.ScalesXAt, View.ScalesYAt, Chart) + uw;

            //    chartPoint.SeriesView = View;

            //    chartPoint.View = View.GetPointView(chartPoint,
            //        View.DataLabels ? View.GetLabelPointFormatter()(chartPoint) : null);

            //    var bubbleView = (IScatterPointView)chartPoint.View;

            //    bubbleView.Diameter = m * (chartPoint.Weight - p1.X) + p1.Y;

            //    chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            //}
        }

        protected override IPointView<TModel, Point<TModel, Weighted2DPoint, ColumnViewModel>, Weighted2DPoint, ColumnViewModel> 
            DefaultPointViewProvider()
        {
            throw new System.NotImplementedException();
        }
    }
}
