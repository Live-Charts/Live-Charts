using System;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The column series class.
    /// </summary>The column series class.
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class BarSeries<TModel>
        : CartesianSeries<TModel, Coordinates.Point, ColumnViewModel, Point<TModel, Coordinates.Point, ColumnViewModel>>, IColumnSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeries{TModel}"/> class.
        /// </summary>
        public BarSeries()
        {
            MaxColumnWidth = 45f;
            ColumnPadding = 2f;

            Charting.BuildFromSettings<IColumnSeries>(this);
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
            int wi = 0, hi = 1, inverted = 1;
            var orientation = Orientation.Horizontal;

            var directionAxis = chart.Dimensions[0][ScalesAt[0]];
            var scaleAxis = chart.Dimensions[1][ScalesAt[1]];

            var uw = chart.Get2DUiUnitWidth(directionAxis, scaleAxis);

            var columnSeries = chart.Series
                .Where(series => series.ScalesAt[1] == ScalesAt[1] &&
                                 series is IColumnSeries)
                .ToList();

            var cw = uw[0] / columnSeries.Count;
            var position = columnSeries.IndexOf(this);

            var overFlow = 0f;

            if (cw > MaxColumnWidth)
            {
                cw = MaxColumnWidth;
            }

            var offsetX = -cw * .5f + uw[0] * .5f;
            var offsetY = 0f;

            ColumnPadding = 5;
            
            var xByPosition = (ColumnPadding + cw) * position;

            if (chart.InvertXy)
            {
                wi = 1;
                hi = 0;
                inverted = 0;
                orientation = Orientation.Vertical;
                offsetX = 0;
                offsetY = -cw * .5f - uw[0] * .5f;
            }

            var columnStart = GetColumnStart(chart, scaleAxis, directionAxis);

            Point<TModel, Coordinates.Point, ColumnViewModel> previous = null;

            foreach (var current in Points)
            {
                var offset = chart.ScaleToUi(current.Coordinate[0][0], directionAxis);

                var columnCorner1 = new[]
                {
                    offset,
                    chart.ScaleToUi(current.Coordinate[1][0], scaleAxis)
                };

                var columnCorner2 = new[]
                {
                    offset + cw,
                    columnStart
                };

                var difference = Vector.SubstractEach2D(columnCorner1, columnCorner2);

                if (current.View == null)
                {
                    current.View = PointViewProvider();
                }

                if (current.View.VisualElement == null)
                {
                    var initialRectangle = new Rectangle();
                    current.ViewModel = new ColumnViewModel(Rectangle.Empty, initialRectangle, orientation);
                }

                var location = new []
                {
                    offset,
                    columnStart - Math.Abs(difference[1]) * inverted
                };

                var vm = new ColumnViewModel(
                    current.ViewModel.To,
                    new Rectangle(
                        location[wi] + offsetX + xByPosition,
                        location[hi] + offsetY,
                        Math.Abs(difference[wi]),
                        Math.Abs(Math.Abs(difference[hi]))),
                    orientation);

                current.InteractionArea = new RectangleInteractionArea(vm.To);

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);

                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }
        }

        /// <inheritdoc />
        protected override IPointView<TModel, Point<TModel, Coordinates.Point, ColumnViewModel>, Coordinates.Point, ColumnViewModel> 
            DefaultPointViewProvider()
        {
            return Charting.Current.UiProvider.GetNerBarPointView<TModel>();
        }

        private static float GetColumnStart(ChartModel chart, Plane target, Plane complementary)
        {
            var value = target.ActualMinValue >= 0 && complementary.ActualMaxValue > 0
                ? target.ActualMinValue
                : (target.ActualMinValue < 0 && complementary.ActualMaxValue <= 0
                    ? target.ActualMaxValue
                    : 0);
            return chart.ScaleToUi(value, target);
        }
    }
}
