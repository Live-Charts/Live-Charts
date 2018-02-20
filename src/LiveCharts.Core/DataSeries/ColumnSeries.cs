using System;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The column series class.
    /// </summary>The column series class.
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ColumnSeries<TModel>
        : CartesianSeries<TModel, Point2D, ColumnViewModel, Point<TModel, Point2D, ColumnViewModel>>, IColumnSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries{TModel}"/> class.
        /// </summary>
        public ColumnSeries()
        {
            MaxColumnWidth = 45;
            ColumnPadding = 2d;
            LiveChartsSettings.Set<IColumnSeries>(this);
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
        public override double[] DefaultPointWidth => new[] {1d, 0d};

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {
            var cartesianChart = (CartesianChartModel) chart;
            var x = cartesianChart.XAxis[ScalesXAt];
            var y = cartesianChart.YAxis[ScalesYAt];

            var xUnitWidth = Math.Abs(chart.ScaleToUi(0, x) - chart.ScaleToUi(x.ActualPointWidth[x.Dimension], x)) - ColumnPadding;
            var columnSeries = chart.Series
                .Where(series => series is IColumnSeries)
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
                var p = chart.ScaleToUi(current.Coordinate, x, y);
                var vm = new ColumnViewModel(
                    p.X + relativeLeft,
                    p.Y < zero
                        ? p.Y
                        : zero,
                    Math.Abs(p.Y - zero),
                    singleColumnWidth - ColumnPadding,
                    zero);

                if (current.View == null)
                {
                    current.View = PointViewProvider();
                }

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);

                current.InteractionArea = new RectangleInteractionArea
                {
                    Top = vm.Top,
                    Left = vm.Left,
                    Height = vm.Height,
                    Width = vm.Width
                };

                if (DataLabels && x.IsInRange(p.X) && y.IsInRange(p.Y))
                {
                    current.View.DrawLabel(
                        current,
                        GetLabelPosition(
                            new Point(p.X, p.Y),
                            new Margin(0),
                            zero,
                            current.View.Label.Measure(current.PackAll()),
                            DataLabelsPosition));
                }

                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }
        }

        /// <inheritdoc />
        protected override IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel> 
            DefaultPointViewProvider()
        {
            return LiveChartsSettings.Current.UiProvider.GetNewColumnView<TModel>();
        }
    }
}
