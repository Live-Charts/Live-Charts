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

using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Brushes;
using LiveCharts.Core.Drawing.Shapes;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Updating;
using System;
using System.Collections.Generic;
using System.Drawing;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The bubble series class.
    /// </summary>
    public class BubbleSeries<TModel>
        : CartesianStrokeSeries<TModel, WeightedCoordinate, ISvgPath, IBrush>, IBubbleSeries
    {
        private double _maxGeometrySize;
        private double _minGeometrySize;

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleSeries{TModel}"/> class.
        /// </summary>
        public BubbleSeries()
        {
            ScalesAt = new[] { 0, 0, 0 };
            MaxGeometrySize = 30f;
            MinGeometrySize = 14f;
            StrokeThickness = 1f;
            Geometry = Geometry.Circle;
            DataLabelFormatter = coordinate =>
                $"{Format.AsMetricNumber(coordinate.X)}, {Format.AsMetricNumber(coordinate.Y)}, {Format.AsMetricNumber(coordinate.Weight)}";
            TooltipFormatter = DataLabelFormatter;
            Charting.BuildFromTheme<IBubbleSeries>(this);
            Charting.BuildFromTheme<ISeries<WeightedCoordinate>>(this);
        }

        /// <inheritdoc />
        public double MaxGeometrySize
        {
            get => _maxGeometrySize;
            set
            {
                _maxGeometrySize = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double MinGeometrySize
        {
            get => _minGeometrySize;
            set
            {
                _minGeometrySize = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Type ThemeKey => typeof(IBubbleSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 0f, 0f };

        /// <inheritdoc />
        public override float PointMargin => (float)MaxGeometrySize;

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel)chart;
            var x = cartesianChart.Dimensions[0][ScalesAt[0]];
            var y = cartesianChart.Dimensions[1][ScalesAt[1]];

            var p1 = new PointF(context.Ranges[2][ScalesAt[2]][0], (float)MinGeometrySize);
            var p2 = new PointF(context.Ranges[2][ScalesAt[2]][1], (float)MaxGeometrySize);

            int xi = 0, yi = 1;
            if (chart.InvertXy)
            {
                xi = 1;
                yi = 0;
            }

            ChartPoint<TModel, WeightedCoordinate, ISvgPath> previous = null;
            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.View.AnimationLine
            };
            float originalDuration = (float)timeLine.Duration.TotalMilliseconds;
            IEnumerable<KeyFrame> originalAnimationLine = timeLine.AnimationLine;
            int i = 0;

            foreach (ChartPoint<TModel, WeightedCoordinate, ISvgPath> current in GetPoints(chart.View))
            {
                float[] p = new[]
                {
                    chart.ScaleToUi(current.Coordinate[0][0], x),
                    chart.ScaleToUi(current.Coordinate[1][0], y),
                    cartesianChart.LinealScale(p1, p2, current.Coordinate.Weight)
                };

                float r = p[2] * .5f;

                var vm = new GeometryPointViewModel
                {
                    Location = new PointF(p[xi] - r, p[yi] - r),
                    Diameter = p[2]
                };

                if (DelayRule != DelayRules.None)
                {
                    timeLine = AnimationExtensions.Delay(
                        // ReSharper disable once PossibleMultipleEnumeration
                        originalDuration, originalAnimationLine, i / (float)PointsCount, DelayRule);
                }

                DrawPointShape(current, timeLine, vm);

                if (DataLabels)
                {
                    DrawPointLabel(current);
                }

                Mapper.EvaluateModelDependentActions(current.Model, current.Shape, current);
                current.InteractionArea = new RectangleInteractionArea(
                    new RectangleF(
                        vm.Location.X,
                        vm.Location.Y,
                        p[2],
                        p[2]));

                previous = current;
                i++;
            }
        }

        private void DrawPointShape(
            ChartPoint<TModel, WeightedCoordinate, ISvgPath> current,
            TimeLine timeline,
            GeometryPointViewModel vm)
        {
            var shape = current.Shape;
            bool isNew = current.Shape == null;

            if (isNew)
            {
                current.Shape = Charting.Settings.UiProvider.GetNewSvgPath(current.Chart.Model);
                shape = current.Shape;
                current.Chart.Content.AddChild(shape, true);
                shape.Left = vm.Location.X;
                shape.Top = vm.Location.Y;
                shape.Width = 0;
                shape.Height = 0;
            }

            shape.Svg = Geometry.Data;
            shape.StrokeDashArray = StrokeDashArray;
            shape.StrokeThickness = StrokeThickness;
            shape.ZIndex = ZIndex;
            shape.Paint(Stroke, Fill);

            float r = vm.Diameter * .5f;

            if (isNew)
            {
                shape.Animate(timeline)
                    .Property(nameof(shape.Left), vm.Location.X + r, vm.Location.X)
                    .Property(nameof(shape.Top), vm.Location.Y + r, vm.Location.Y)
                    .Property(nameof(shape.Width), 0, vm.Diameter)
                    .Property(nameof(shape.Height), 0, vm.Diameter)
                    .Begin();
            }
            else
            {
                shape.Animate(timeline)
                    .Property(nameof(shape.Left), shape.Left, vm.Location.X)
                    .Property(nameof(shape.Top), shape.Top, vm.Location.Y)
                    .Begin();
            }
        }
    }
}
