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
using System.Collections.Generic;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Brushes;
using LiveCharts.Core.Drawing.Shapes;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Updating;
#if NET45
using Font = LiveCharts.Core.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The base bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
    public abstract class BaseBarSeries<TModel, TCoordinate>
        : CartesianStrokeSeries<TModel, TCoordinate, IRectangle, IBrush>, IBarSeries
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

            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.View.AnimationLine
            };
            float originalDuration = (float)timeLine.Duration.TotalMilliseconds;
            IEnumerable<KeyFrame> originalAnimationLine = timeLine.AnimationLine;
            int i = 0;

            foreach (ChartPoint<TModel, TCoordinate, IRectangle> current in GetPoints(chart.View))
            {
                if (DelayRule != DelayRules.None)
                {
                    timeLine = AnimationExtensions.Delay(
                        // ReSharper disable once PossibleMultipleEnumeration
                        originalDuration, originalAnimationLine, i / (float)PointsCount, DelayRule);
                }

                var vm = BuildModel(
                    current, context, chart, directionAxis,
                    scaleAxis, cw, pivot, byBarOffset,
                    positionOffset, orientation, h, w);

                DrawPointShape(current, timeLine, vm, pivot);

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
        protected abstract RectangleViewModel BuildModel(
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
            TimeLine timeline,
            RectangleViewModel vm,
            float pivot)
        {
            var shape = current.Shape;
            var isNewShape = false;

            if (current.Shape == null)
            {
                current.Shape = Charting.Settings.UiProvider.GetNewRectangle(current.Chart.Model);
                shape = current.Shape;
                current.Chart.Content.AddChild(shape, true);
                shape.Left = vm.From.Left;
                shape.Top = vm.From.Top;
                shape.Width = vm.From.Width;
                shape.Height = vm.From.Height;
                isNewShape = true;

                void AnimateOnDispose(IChartView view, object instance, bool force)
                {
                    current.Disposed -= AnimateOnDispose;

                    if (force)
                    {
                        current.Chart.Content.DisposeChild(shape, true);
                        //current.Chart.Content.DisposeChild(Label, true);
                        return;
                    }

                    // if not forced, animate the exit...

                    var animation = shape.Animate(timeline)
                        .Property(nameof(shape.Top), shape.Top, pivot)
                        .Property(nameof(shape.Height), shape.Height, 0);

                    animation.Then((sender, args) =>
                    {
                        current.Chart.Content?.DisposeChild(shape, true);
                        // chart.Content?.DisposeChild(Label, true);
                        animation.Dispose();
                        animation = null;
                    }).Begin();
                }

                current.Disposed += AnimateOnDispose;
            }

            // map properties
            shape.StrokeDashArray = StrokeDashArray;
            shape.StrokeThickness = StrokeThickness;
            shape.ZIndex = ZIndex;
            shape.Paint(Stroke, Fill);

            float radius = (vm.Orientation == Orientation.Horizontal ? vm.To.Width : vm.To.Height) * .4f;
            shape.XRadius = radius;
            shape.YRadius = radius;

            shape
                .Animate(timeline)
                .Property(nameof(shape.Left), shape.Left, vm.To.Left)
                .Property(nameof(shape.Width), shape.Width, vm.To.Width)
                .Property(nameof(shape.Top), shape.Top, vm.To.Top)
                .Property(nameof(shape.Height), !isNewShape ? shape.Height : vm.From.Height, vm.To.Height)
                .Begin();
        }
    }
}