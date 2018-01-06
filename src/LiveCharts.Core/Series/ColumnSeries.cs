using System.Linq;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// 
    /// </summary>The column series class.
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ColumnSeries<TModel> : CartesianSeries<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries{TModel}"/> class.
        /// </summary>
        public ColumnSeries()
            : base(LiveChartsConstants.ColumnSeries)
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
                .Where(series => series.Defaults.Type == "")
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

            ChartPoint previous = null;
            foreach (var chartPoint in FetchData(chart))
            {
                if (chartPoint.View == null)
                {
                    chartPoint.View = Defaults.PointViewProvider();
                }

                chartPoint.View.Draw(chartPoint, previous, chart.View);
                previous = chartPoint;
            }

        }
    }
}