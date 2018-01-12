using System;
using System.Linq;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// A Column series.
    /// </summary>The column series class.
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ColumnSeries<TModel>
        : CartesianSeries<TModel, Point2D, ColumnViewModel, Point<TModel, Point2D, ColumnViewModel>>
        // shouldn't the next line compile too??? probably a bug for .Net team
        // where TView : ChartPointView<TModel, TPoint, Point2D, ColumnViewModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries{TModel}"/> class.
        /// </summary>
        public ColumnSeries()
            : base(SeriesConstants.Column)
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

        /// <inheritdoc cref="OnUpdateView"/>
        protected override void OnUpdateView(ChartModel chart)
        {
            var viewProvider = PointViewProvider ?? LiveChartsSettings.Current.UiProvider.ColumnViewProvider<TModel>;
            var mapper = Mapper ?? LiveChartsSettings.GetMapper<TModel, Point2D>();

            var cartesianChart = (CartesianChartModel) chart;
            var x = cartesianChart.XAxis[ScalesXAt];
            var y = cartesianChart.YAxis[ScalesYAt];

            var xUnitWidth = chart.ScaleToUi(x.Unit, x) - ColumnPadding;
            var columnSeries = chart.Series
                .Where(series => series.Key == SeriesConstants.Column)
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
                ? y.ActualMinValue                                      // then use axisYMin
                : (y.ActualMinValue < 0 && y.ActualMaxValue <= 0        // both negative
                    ? y.ActualMaxValue                                  // then use axisYMax
                    : 0);
            var zero = chart.ScaleToUi(startAt, y);

            Point<TModel, Point2D, ColumnViewModel> previous = null;
            foreach (var current in Points)
            {
                if (current.View == null)
                {
                    current.View = viewProvider();
                }

                var p = chart.ScaleToUi(current.Coordinate, x, y);

                current.View.Draw(
                    current,
                    previous,
                    chart.View,
                    new ColumnViewModel(
                        p.X + relativeLeft,
                        p.Y < zero
                            ? p.Y
                            : zero,
                        Math.Abs(p.Y - zero),
                        singleColumnWidth - ColumnPadding,
                        zero));

                mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }

        }
    }
}
