using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Updating;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The base bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    public abstract class BaseBarSeries<TModel, TCoordinate, TSeries>
        : CartesianStrokeSeries<TModel, TCoordinate, BarViewModel, TSeries>, IBarSeries
        where TCoordinate : ICoordinate
        where TSeries : class, ISeries
    {
        private double _pivot;
        private double _barPadding;
        private double _maxColumnWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBarSeries{TModel,TCoordinate, TSeries}"/> class.
        /// </summary>
        protected BaseBarSeries()
        {
            MaxColumnWidth = 45f;
            BarPadding = 6f;
            Charting.BuildFromSettings<IBarSeries>(this);
        }

        /// <inheritdoc />
        public double BarPadding
        {
            get => _barPadding;
            set
            {
                _barPadding = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double MaxColumnWidth
        {
            get => _maxColumnWidth;
            set
            {
                _maxColumnWidth = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double Pivot
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

            var cw = (uw[0] - ((float) BarPadding) * barsCount) / barsCount;
            var position = context.GetBarIndex(ScalesAt[1], this);

            if (cw > MaxColumnWidth)
            {
                cw = (float) MaxColumnWidth;
            }

            var bp = (float) BarPadding;

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
                byBarOffset[1] = cw * .5f - uw[0] * .5f;
            }

            positionOffset[w] = (bp + cw) * position - (bp + cw) * ((barsCount - 1) * .5f);
            

            var columnStart = GetColumnStart(chart, scaleAxis, directionAxis);

            Point<TModel, TCoordinate, BarViewModel, TSeries> previous = null;

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
        protected abstract void BuildModel(
            Point<TModel, TCoordinate, BarViewModel, TSeries> current, UpdateContext context, ChartModel chart, Plane directionAxis, Plane scaleAxis,
            float cw, float columnStart, float[] byBarOffset, float[] positionOffset, Orientation orientation,
            int h, int w);

        private float GetColumnStart(ChartModel chart, Plane target, Plane complementary)
        {
            var p = (float) Pivot;

            var value = target.ActualMinValue >= p && complementary.ActualMaxValue > p
                ? target.ActualMinValue
                : (target.ActualMinValue < p && complementary.ActualMaxValue <= p
                    ? target.ActualMaxValue
                    : p);
            return chart.ScaleToUi(value, target);
        }
    }
}