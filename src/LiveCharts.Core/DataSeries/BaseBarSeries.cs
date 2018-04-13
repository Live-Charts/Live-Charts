using System;
using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Updater;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The base bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">The type of the point.</typeparam>
    /// <typeparam name="TCoordinate">The type fo the coordinate.</typeparam>
    public abstract class BaseBarSeries<TModel, TCoordinate, TPoint>
        : CartesianSeries<TModel, TCoordinate, BarViewModel, TPoint>, IBarSeries
        where TCoordinate : ICoordinate
        where TPoint : Point<TModel, TCoordinate, BarViewModel>, new()
    {
        private float _pivot;
        private float _barPadding;
        private float _maxColumnWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBarSeries{TModel,TCoordinate,TPoint}"/> class.
        /// </summary>
        protected BaseBarSeries()
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
        public float Pivot
        {
            get => _pivot;
            set
            {
                _pivot = value;
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
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            int w = 0, h = 1;
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

            var byBarOffset = new[]
            {
                -cw * .5f + uw[0] * .5f,
                0f
            };

            var positionOffset = new float[2];

            if (chart.InvertXy)
            {
                w = 1;
                h = 0;
                orientation = Orientation.Vertical;
                byBarOffset[0] = 0;
                byBarOffset[1] = -cw * .5f - uw[0] * .5f;
            }

            positionOffset[w] =
                (BarPadding + cw) * position - (BarPadding + cw) * ((barsCount - 1) * .5f);

            var columnStart = GetColumnStart(chart, scaleAxis, directionAxis);

            TPoint previous = null;

            foreach (var current in Points)
            {
                if (current.View == null)
                {
                    current.View = ViewProvider.Getter();
                }

                BuildModel(
                    current, context, chart, directionAxis,
                    scaleAxis, cw, columnStart, byBarOffset,
                    positionOffset, orientation, h, w);

                current.InteractionArea = new RectangleInteractionArea(current.ViewModel.To);
                current.View.DrawShape(current, previous);
                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }
        }

        /// <summary>
        /// Offsets the by stack.
        /// </summary>
        /// <returns></returns>
        protected virtual void BuildModel(
            TPoint current, UpdateContext context, ChartModel chart, Plane directionAxis, Plane scaleAxis,
            float cw, float columnStart, float[] byBarOffset, float[] positionOffset, Orientation orientation,
            int h, int w)
        {
            var currentOffset = chart.ScaleToUi(current.Coordinate[0][0], directionAxis);

            var columnCorner1 = new[]
            {
                currentOffset,
                chart.ScaleToUi(current.Coordinate[1][0], scaleAxis)
            };

            var columnCorner2 = new[]
            {
                currentOffset + cw,
                columnStart
            };

            var difference = Perform.SubstractEach2D(columnCorner1, columnCorner2);

            var location = new[]
            {
                currentOffset,
                columnStart + (columnCorner1[1] < columnStart ? difference[1] : 0f)
            };

            if (current.View.VisualElement == null)
            {
                var initialRectangle = chart.InvertXy
                    ? new RectangleF(
                        columnStart,
                        location[h] + byBarOffset[1] + positionOffset[1],
                        0f,
                        Math.Abs(difference[h]))
                    : new RectangleF(
                        location[w] + byBarOffset[0] + positionOffset[0],
                        columnStart,
                        Math.Abs(difference[w]),
                        0f);
                current.ViewModel = new BarViewModel(RectangleF.Empty, initialRectangle, orientation);
            }

            current.ViewModel = new BarViewModel(
                current.ViewModel.To,
                new RectangleF(
                    location[w] + byBarOffset[0] + positionOffset[0],
                    location[h] + byBarOffset[1] + positionOffset[1],
                    Math.Abs(difference[w]),
                    Math.Abs(difference[h])),
                orientation);
        }

        private float GetColumnStart(ChartModel chart, Plane target, Plane complementary)
        {
            var value = target.ActualMinValue >= Pivot && complementary.ActualMaxValue > Pivot
                ? target.ActualMinValue
                : (target.ActualMinValue < Pivot && complementary.ActualMaxValue <= Pivot
                    ? target.ActualMaxValue
                    : Pivot);
            return chart.ScaleToUi(value, target);
        }
    }
}