using System;
using System.Drawing;
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
    public class ColumnSeries<TModel>
        : CartesianSeries<TModel, Coordinates.Point, ColumnViewModel, Point<TModel, Coordinates.Point, ColumnViewModel>>, IColumnSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries{TModel}"/> class.
        /// </summary>
        public ColumnSeries()
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
            var xi = 0;
            var yi = 1;

            var x = chart.Dimensions[xi][ScalesAt[xi]];
            var y = chart.Dimensions[yi][ScalesAt[yi]];

            var uw = chart.Get2DUiUnitWidth(x, y);

            var columnSeries = chart.Series
                .Where(series => series is IColumnSeries)
                .ToList();

            var cw = uw[xi] / columnSeries.Count;

            var overFlow = 0f;
            var seriesPosition = columnSeries.IndexOf(this);
            if (cw > MaxColumnWidth)
            {
                overFlow = (cw - MaxColumnWidth) * columnSeries.Count / 2f;
                cw = MaxColumnWidth;
            }

            var zero = GetZero(chart, chart.InvertXy ? x : y, x, y);

            Point<TModel, Coordinates.Point, ColumnViewModel> previous = null;
            foreach (var current in Points)
            {
                var offset = chart.ScaleToUi(current.Coordinate[xi][0], x);

                var c0 = new[]
                {
                    offset,
                    chart.ScaleToUi(current.Coordinate[yi][0], y)
                };

                var c1 = new[]
                {
                    offset + cw,
                    zero
                };

                var difference = Vector.SubstractEach2D(c0, c1);

                if (current.View == null)
                {
                    current.View = PointViewProvider();
                }

                var h = Math.Abs(difference[yi]);
                var w = Math.Abs(Math.Abs(difference[xi]));

                var vm = new ColumnViewModel(
                    Column.Empty,
                    new Column(
                        offset,
                        zero - h,
                        h,
                        w));

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);

                current.InteractionArea = new RectangleInteractionArea();

                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }
        }

        /// <inheritdoc />
        protected override IPointView<TModel, Point<TModel, Coordinates.Point, ColumnViewModel>, Coordinates.Point, ColumnViewModel> 
            DefaultPointViewProvider()
        {
            return Charting.Current.UiProvider.GetNewColumnView<TModel>();
        }

        private void inverted(ChartModel chart)
        {
            var i = 0;
            var j = 1;

            var x = chart.Dimensions[i][ScalesAt[i]];
            var y = chart.Dimensions[j][ScalesAt[j]];

            var uw = chart.Get2DUiUnitWidth(x, y);

            var columnSeries = chart.Series
                .Where(series => series is IColumnSeries)
                .ToList();
            var cw = uw[i] / columnSeries.Count;

            var overFlow = 0f;
            var seriesPosition = columnSeries.IndexOf(this);
            if (cw > MaxColumnWidth)
            {
                overFlow = (cw - MaxColumnWidth) * columnSeries.Count / 2f;
                cw = MaxColumnWidth;
            }

            var startAt = x.ActualMinValue >= 0 && y.ActualMaxValue > 0
                ? y.ActualMinValue
                : (x.ActualMinValue < 0 && y.ActualMaxValue <= 0
                    ? y.ActualMaxValue
                    : 0);
            var zero = chart.ScaleToUi(startAt, y);

            Point<TModel, Coordinates.Point, ColumnViewModel> previous = null;
            foreach (var current in Points)
            {
                var offset = chart.ScaleToUi(current.Coordinate[i][0], x) - uw[i]*.5f;

                var c0 = new[]
                {
                    chart.ScaleToUi(current.Coordinate[j][0], y),
                    offset
                };

                var c1 = new[]
                {
                    zero,
                    offset + cw
                };

                var difference = Vector.SubstractEach2D(c0, c1);

                if (current.View == null)
                {
                    current.View = PointViewProvider();
                }

                var vm = new ColumnViewModel(
                    Column.Empty,
                    new Column(
                        zero,
                        offset - .5f * cw,
                        Math.Abs(difference[j]),
                        Math.Abs(difference[i])));

                current.ViewModel = vm;
                current.View.DrawShape(current, previous);

                current.InteractionArea = new RectangleInteractionArea();

                Mapper.EvaluateModelDependentActions(current.Model, current.View.VisualElement, current);

                previous = current;
            }
        }

        private static float GetZero(ChartModel chart, Plane target, Plane x, Plane y)
        {
            var value = x.ActualMinValue >= 0 && y.ActualMaxValue > 0
                ? target.ActualMinValue
                : (x.ActualMinValue < 0 && y.ActualMaxValue <= 0
                    ? target.ActualMaxValue
                    : 0);
            return chart.ScaleToUi(value, target);
        }
    }
}
