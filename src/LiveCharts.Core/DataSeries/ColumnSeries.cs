using System;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;
using Point = LiveCharts.Core.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

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
            MaxColumnWidth = 45f;
            ColumnPadding = 2f;
            LiveChartsSettings.Set<IColumnSeries>(this);
        }

        /// <inheritdoc />
        public float MaxColumnWidth { get; set; }

        /// <inheritdoc />
        public float ColumnPadding { get; set; }

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] {1f, 0f};

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {
            var cartesianChart = (CartesianChartModel) chart;
            var x = cartesianChart.XAxis[ScalesXAt];
            var y = cartesianChart.YAxis[ScalesYAt];

            var xUnitWidth = Math.Abs(chart.ScaleToUi(0, x) - chart.ScaleToUi(x.ActualPointWidth[x.Dimension], x)) -
                             ColumnPadding;
            var columnSeries = chart.Series
                .Where(series => series is IColumnSeries)
                .ToList();
            var singleColumnWidth = xUnitWidth / columnSeries.Count;

            var overFlow = 0f;
            var seriesPosition = columnSeries.IndexOf(this);
            if (singleColumnWidth > MaxColumnWidth)
            {
                overFlow = (singleColumnWidth - MaxColumnWidth) * columnSeries.Count / 2f;
                singleColumnWidth = MaxColumnWidth;
            }

            var relativeLeft = ColumnPadding + overFlow + singleColumnWidth * seriesPosition;

            var startAt = x.ActualMinValue >= 0 && y.ActualMaxValue > 0 // both positive
                ? y.ActualMinValue // then use axisYMin
                : (y.ActualMinValue < 0 && y.ActualMaxValue <= 0 // both negative
                    ? y.ActualMaxValue // then use axisYMax
                    : 0);
            var zero = chart.ScaleToUi(startAt, y);

            Point<TModel, Point2D, ColumnViewModel> previous = null;
            foreach (var current in Points)
            {
                var p = new[]
                {
                    chart.ScaleToUi(current.Coordinate.X, x),
                    chart.ScaleToUi(current.Coordinate.Y, y)
                };

                var isNew = current.View != null && current.View.VisualElement == null;

                var initialState = isNew
                    ? new RectangleF(p[0] + relativeLeft,
                        p[1] < zero
                            ? p[1]
                            : zero,
                        Math.Abs(p[1] - zero),
                        0)
                    : Rectangle.Empty;

                var vm = new ColumnViewModel(
                    p[0] + relativeLeft,
                    p[1] < zero
                        ? p[1]
                        : zero,
                    Math.Abs(p[1] - zero),
                    singleColumnWidth - ColumnPadding,
                    zero,
                    initialState);

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

                if (DataLabels && x.IsInRange(p[0]) && y.IsInRange(p[1]))
                {
                    current.View.DrawLabel(
                        current,
                        GetLabelPosition(
                            new Point(p[0], p[1]),
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
