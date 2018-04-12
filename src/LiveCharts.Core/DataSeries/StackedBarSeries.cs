using System;
using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The stacked bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="CartesianSeries{TModel, StackedCoordinate, BarViewModel, Point}" />
    /// <seealso cref="LiveCharts.Core.Abstractions.DataSeries.IBarSeries" />
    public class StackedBarSeries<TModel>
        : CartesianSeries<TModel, StackedCoordinate, BarViewModel, Point<TModel, StackedCoordinate, BarViewModel>>,
            IBarSeries
    {
        private static ISeriesViewProvider<TModel, StackedCoordinate, BarViewModel> _provider;
        private float _barPadding;
        private float _maxColumnWidth;
        private int _stackIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedBarSeries{TModel}"/> class.
        /// </summary>
        public StackedBarSeries()
        {
            MaxColumnWidth = 45f;
            BarPadding = 6f;
            Charting.BuildFromSettings<IBarSeries>(this);
        }

        /// <inheritdoc />
        public float BarPadding
        {
            get => _barPadding;
            set
            {
                _barPadding = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public float MaxColumnWidth
        {
            get => _maxColumnWidth;
            set
            {
                _maxColumnWidth = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        int ISeries.GroupingIndex => StackIndex;

        /// <summary>
        /// Gets or sets the stack index, bars that shares the same indexes will be stacked together, 
        /// if set to -1 the series won't be stacked with any other series.
        /// </summary>
        /// <value>
        /// The index of the stack.
        /// </value>
        public int StackIndex
        {
            get => _stackIndex;
            set
            {
                _stackIndex = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Type ResourceKey => typeof(IBarSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 1f, 0f };

        /// <inheritdoc />
        public override float[] PointMargin => new[] { 0f, 0f };

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, StackedCoordinate, BarViewModel>
            DefaultViewProvider =>
            _provider ?? (_provider = Charting.Current.UiProvider.StackedBarViewProvider<TModel>());

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            int wi = 0, hi = 1, inverted = 1;
            var orientation = Orientation.Horizontal;

            var directionAxis = chart.Dimensions[0][ScalesAt[0]];
            var scaleAxis = chart.Dimensions[1][ScalesAt[1]];

            var uw = chart.Get2DUiUnitWidth(directionAxis, scaleAxis);

            var barsCount = context.GetBarsCount(ScalesAt[1]);

            var cw = (uw[0] - BarPadding * barsCount) / barsCount;
            var position = context.GetBarIndex(ScalesAt[1], this);

            if (cw > MaxColumnWidth)
            {
                cw = MaxColumnWidth;
            }

            var offsetX = -cw * .5f + uw[0] * .5f;
            var offsetY = 0f;

            var positionOffset = new float[2];

            if (chart.InvertXy)
            {
                wi = 1;
                hi = 0;
                inverted = 0;
                orientation = Orientation.Vertical;
                offsetX = 0;
                offsetY = -cw * .5f - uw[0] * .5f;
            }

            positionOffset[wi] =
                (BarPadding + cw) * position - (BarPadding + cw) * ((barsCount - 1) * .5f);

            var columnStart = GetColumnStart(chart, scaleAxis, directionAxis);

            Point<TModel, StackedCoordinate, BarViewModel> previous = null;

            foreach (var current in Points)
            {
                var offset = chart.ScaleToUi(current.Coordinate[0][0], directionAxis);

                float stack;

                unchecked
                {
                    stack = context.GetStack(StackIndex, (int) current.Coordinate.Key, current.Coordinate[1][0] >= 0);
                }

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

                var difference = Perform.SubstractEach2D(columnCorner1, columnCorner2);

                if (current.View == null)
                {
                    current.View = ViewProvider.Getter();
                }

                var location = new[]
                {
                    offset,
                    columnStart - Math.Abs(difference[1]) * inverted
                };

                var start = current.Coordinate.From / stack;
                var end = current.Coordinate.To / stack;

                if (current.View.VisualElement == null)
                {
                    var initialRectangle = chart.InvertXy
                        ? new RectangleF(
                            columnStart,
                            location[hi] + offsetY + positionOffset[1],
                            0f,
                            Math.Abs(difference[hi]))
                        : new RectangleF(
                            location[wi] + offsetX + positionOffset[0],
                            columnStart,
                            Math.Abs(difference[wi]),
                            0f);
                    current.ViewModel = new BarViewModel(RectangleF.Empty, initialRectangle, orientation);
                }

                var vm = new BarViewModel(
                    current.ViewModel.To,
                    new RectangleF(
                        location[wi] + offsetX + positionOffset[0],
                        location[hi] + offsetY + positionOffset[1],
                        Math.Abs(difference[wi]),
                        Math.Abs(difference[hi])),
                    orientation);

                current.InteractionArea = new RectangleInteractionArea(vm.To);
                current.ViewModel = vm;
                current.View.DrawShape(current, previous);
                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }
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