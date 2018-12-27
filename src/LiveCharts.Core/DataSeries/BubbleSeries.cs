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

using LiveCharts.Animations;
using LiveCharts.Charts;
using LiveCharts.Coordinates;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction;
using LiveCharts.Interaction.Areas;
using LiveCharts.Interaction.Points;
using LiveCharts.Updating;
using System;
using System.Collections.Generic;
using System.Drawing;

#endregion

namespace LiveCharts.DataSeries
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
            Global.Settings.BuildFromTheme<IBubbleSeries>(this);
            Global.Settings.BuildFromTheme<ISeries<WeightedCoordinate>>(this);
        }

        /// <inheritdoc />
        public double MaxGeometrySize
        {
            get => _maxGeometrySize;
            set
            {
                _maxGeometrySize = value;
                OnPropertyChanged(nameof(MaxGeometrySize));
            }
        }

        /// <inheritdoc />
        public double MinGeometrySize
        {
            get => _minGeometrySize;
            set
            {
                _minGeometrySize = value;
                OnPropertyChanged(nameof(MinGeometrySize));
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

            ChartPoint<TModel, WeightedCoordinate, ISvgPath>? previous = null;
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
                    new RectangleD(
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
            var isNew = current.Shape == null;

            if (current.Shape == null)
            {
                current.Shape = UIFactory.GetNewSvgPath(current.Chart.Model);
                current.Chart.Canvas.AddChild(current.Shape, true);
                current.Shape.Left = vm.Location.X;
                current.Shape.Top = vm.Location.Y;
                current.Shape.Width = 0;
                current.Shape.Height = 0;
            }

            current.Shape.Svg = Geometry.Data;
            current.Shape.StrokeDashArray = StrokeDashArray;
            current.Shape.StrokeThickness = StrokeThickness;
            current.Shape.ZIndex = ZIndex;
            current.Shape.Paint(Stroke, Fill);

            var r = vm.Diameter * .5f;

            if (isNew)
            {
                current.Shape.Animate(timeline)
                    .Property(nameof(ISvgPath.Left), vm.Location.X + r, vm.Location.X)
                    .Property(nameof(ISvgPath.Top), vm.Location.Y + r, vm.Location.Y)
                    .Property(nameof(ISvgPath.Width), 0, vm.Diameter)
                    .Property(nameof(ISvgPath.Height), 0, vm.Diameter)
                    .Begin();
            }
            else
            {
                current.Shape.Animate(timeline)
                    .Property(nameof(ISvgPath.Left), current.Shape.Left, vm.Location.X)
                    .Property(nameof(ISvgPath.Top), current.Shape.Top, vm.Location.Y)
                    .Begin();
            }
        }
    }
}
