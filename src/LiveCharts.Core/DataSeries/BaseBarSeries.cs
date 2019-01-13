#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System;
using LiveCharts.Animations;
using LiveCharts.Charts;
using LiveCharts.Coordinates;
using LiveCharts.Dimensions;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Drawing.Styles;
using LiveCharts.Interaction.Areas;
using LiveCharts.Interaction.Points;
using LiveCharts.Updating;
#if NET45
using Font = LiveCharts.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.DataSeries
{
    /// <summary>
    /// The base bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
    public abstract class BaseBarSeries<TModel, TCoordinate>
        : CartesianStrokeSeries<TModel, TCoordinate, IRectangle>, IBarSeries
        where TCoordinate : ICoordinate
    {
        private double _pivot;
        private double _barPadding;
        private double _maxColumnWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBarSeries{TModel, TCoordinate}"/> class.
        /// </summary>
        protected BaseBarSeries()
        {
            MaxColumnWidth = 45f;
            BarPadding = 6f;
        }

        /// <inheritdoc />
        public double BarPadding
        {
            get => _barPadding;
            set
            {
                _barPadding = value;
                OnPropertyChanged(nameof(BarPadding));
            }
        }

        /// <inheritdoc />
        public double MaxColumnWidth
        {
            get => _maxColumnWidth;
            set
            {
                _maxColumnWidth = value;
                OnPropertyChanged(nameof(MaxColumnWidth));
            }
        }

        /// <inheritdoc />
        public double Pivot
        {
            get => _pivot;
            set
            {
                _pivot = value;
                OnPropertyChanged(nameof(Pivot));
            }
        }

        /// <inheritdoc />
        public override Type ThemeKey => typeof(IBarSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 1f, 0f };

        /// <inheritdoc />
        public override float PointMargin => 0f;

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            int w = 0, h = 1;
            var orientation = Orientation.Horizontal;

            var directionAxis = chart.Dimensions[0][ScalesAt[0]];
            var scaleAxis = chart.Dimensions[1][ScalesAt[1]];

            float[] uw = chart.Get2DUiUnitWidth(directionAxis, scaleAxis);

            int barsCount = context.GetBarsCount(ScalesAt[1]);

            float cw = (uw[0] - (float)BarPadding * barsCount) / barsCount;
            int position = context.GetBarIndex(ScalesAt[1], this);

            if (cw > MaxColumnWidth)
            {
                cw = (float)MaxColumnWidth;
            }

            float bp = (float)BarPadding;

            float[] byBarOffset = new[]
            {
                -cw * .5f,
                0f
            };

            float[] positionOffset = new float[2];

            if (chart.InvertXy)
            {
                w = 1;
                h = 0;
                orientation = Orientation.Vertical;
                byBarOffset[0] = 0;
                byBarOffset[1] = -cw * .5f;
            }

            positionOffset[w] = (bp + cw) * position - (bp + cw) * ((barsCount - 1) * .5f);

            float pivot = GetColumnStart(chart, scaleAxis, directionAxis);

            var animation = AnimatableArguments.BuildFrom(chart.View, this);
            int i = 0;

            foreach (ChartPoint<TModel, TCoordinate, IRectangle> current in GetPoints(chart.View))
            {
                if (DelayRule != DelayRules.None)
                {
                    animation.SetDelay(DelayRule, i / (double)PointsCount);
                }

                var vm = BuildModel(
                    current, context, chart, directionAxis,
                    scaleAxis, cw, pivot, byBarOffset,
                    positionOffset, orientation, h, w);

                DrawPointShape(current, animation, vm, pivot);

                if (DataLabels)
                {
                    DrawPointLabel(current);
                }

                Mapper.EvaluateModelDependentActions(current.Model, current.Shape, current);
                current.InteractionArea = new RectangleInteractionArea(vm.To);
                i++;
            }
        }

        /// <summary>
        /// Offsets the by stack.
        /// </summary>
        /// <returns></returns>
        internal abstract RectangleViewModel BuildModel(
            ChartPoint<TModel, TCoordinate, IRectangle> current,
            UpdateContext context,
            ChartModel chart,
            Plane directionAxis,
            Plane scaleAxis,
            float cw,
            float columnStart,
            float[] byBarOffset,
            float[] positionOffset,
            Orientation orientation,
            int h,
            int w);

        private float GetColumnStart(ChartModel chart, Plane target, Plane complementary)
        {
            float p = (float)Pivot;

            double value = target.ActualMinValue >= p && complementary.ActualMaxValue > p
                ? target.ActualMinValue
                : (target.ActualMinValue < p && complementary.ActualMaxValue <= p
                    ? target.ActualMaxValue
                    : p);
            return chart.ScaleToUi(value, target);
        }

        private void DrawPointShape(
            ChartPoint<TModel, TCoordinate, IRectangle> current,
            AnimatableArguments animationArgs,
            RectangleViewModel vm,
            float pivot)
        {
            var isNewShape = false;

            if (current.Shape == null)
            {
                current.Shape = UIFactory.GetNewRectangle(current.Chart.Model);
                current.Shape.FlushToCanvas(current.Chart.Canvas, true);
                current.Shape.Left = vm.From.Left;
                current.Shape.Top = vm.From.Top;
                current.Shape.Width = vm.From.Width;
                current.Shape.Height = vm.From.Height;
                isNewShape = true;
            }

            // map properties
            current.Shape.StrokeDashArray = StrokeDashArray;
            current.Shape.StrokeThickness = StrokeThickness;
            current.Shape.ZIndex = ZIndex;
            current.Shape.Fill = Fill;
            current.Shape.Stroke = Stroke;

            var radius = (vm.Orientation == Orientation.Horizontal ? vm.To.Width : vm.To.Height) * .4;
            current.Shape.XRadius = radius;
            current.Shape.YRadius = radius;

            current.Shape
                .Animate(animationArgs)
                .Property(nameof(IShape.Left), current.Shape.Left, vm.To.Left)
                .Property(nameof(IShape.Width), current.Shape.Width, vm.To.Width)
                .Property(nameof(IShape.Top), current.Shape.Top, vm.To.Top)
                .Property(nameof(IShape.Height), !isNewShape ? current.Shape.Height : vm.From.Height, vm.To.Height)
                .Begin();
        }
    }
}