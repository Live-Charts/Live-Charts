using System.Linq;
using LiveCharts.Core.Abstractions.PointViews;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// 
    /// </summary>The column series class.
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The user interface view.</typeparam>
    /// <typeparam name="TChartPoint">the type of the chart point.</typeparam>
    public abstract class ColumnSeries<TModel, TChartPoint, TView>
        : CartesianSeries<TModel, Point2D, ColumnViewModel, TChartPoint>
        where TChartPoint : ChartPoint<TModel, Point2D, ColumnViewModel>, new()
        where TView : ChartPointView<TModel, ChartPoint<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel>, new()
        // shouldn't the next line compile too??? probably a bug for .Net team
        // where TView : ChartPointView<TModel, TChartPoint, Point2D, ColumnViewModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries{TModel, TChartPoint, TView}"/> class.
        /// </summary>
        protected ColumnSeries()
            : base(SeriesKeys.Column)
        {
        }

        /// <summary>
        /// Gets or sets the maximum width of the column.
        /// </summary>
        /// <value>
        /// The maximum width of the column.
        /// </value>
        public double MaxColumnWidth { get; set; }

        /// <summary>
        /// Gets or sets the column padding.
        /// </summary>
        /// <value>
        /// The column padding.
        /// </value>
        public double ColumnPadding { get; set; }

        /// <inheritdoc />
        protected override void OnUpdateView(ChartModel chart)
        {
            var cartesianChart = (CartesianChartModel) chart;
            var x = cartesianChart.XAxis[ScalesXAt];
            var y = cartesianChart.YAxis[ScalesYAt];

            var xUnitWidth = chart.ScaleTo(x.Unit, x) - ColumnPadding;
            var columnSeries = chart.Series
                .Where(series => series.Key == SeriesKeys.Column)
                .ToList();
            var singleColumnWidth = xUnitWidth / columnSeries.Count;

            double overFlow = 0;
            var seriesPosition = columnSeries.IndexOf(this);
            if (singleColumnWidth > MaxColumnWidth)
            {
                overFlow = (singleColumnWidth - MaxColumnWidth) * columnSeries.Count / 2;
                singleColumnWidth = MaxColumnWidth;
            }

            var relativeLeft = ColumnPadding + overFlow + singleColumnWidth * seriesPosition;

            var startAt = x.ActualMinValue >= 0 && y.ActualMaxValue > 0 // both positive
                ? y.ActualMinValue // then use axisYMin
                : (y.ActualMinValue < 0 && y.ActualMaxValue <= 0 // both negative
                    ? y.ActualMaxValue // then use axisYMax
                    : 0);

            var zero = chart.ScaleTo(startAt, y);

            TChartPoint previous = null;
            foreach (var chartPoint in Points)
            {
                if (chartPoint.View == null)
                {
                    chartPoint.View = new TView();
                }
                chartPoint.View.Draw(chartPoint, new ColumnViewModel(), previous, chart.View);
                previous = chartPoint;
            }

        }
    }
}
