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
    /// The heat series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="CartesianStrokeSeries{TModel,TCoordinate,TPointShape, TBrush}" />
    /// <seealso cref="IHeatSeries" />
    public class HeatSeries<TModel>
        : CartesianStrokeSeries<TModel, WeightedCoordinate, IColoredShape, ISolidColorBrush>, IHeatSeries
    {
        private IEnumerable<GradientStop>? _gradient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatSeries{TModel}"/> class.
        /// </summary>
        public HeatSeries()
        {
            ScalesAt = new[] { 0, 0, 0 };
            DefaultFillOpacity = .2f;
            DataLabelFormatter = coordinate => $"{Format.AsMetricNumber(coordinate.Weight)}";
            TooltipFormatter = DataLabelFormatter;
            Global.Settings.BuildFromTheme<IHeatSeries>(this);
            Global.Settings.BuildFromTheme<ISeries<WeightedCoordinate>>(this);
        }

        /// <inheritdoc />
        public IEnumerable<GradientStop>? Gradient
        {
            get => _gradient;
            set
            {
                _gradient = value;
                OnPropertyChanged(nameof(Gradient));
            }
        }

        /// <inheritdoc />
        public override Type ThemeKey => typeof(IHeatSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 1f, 1f };

        /// <inheritdoc />
        public override float PointMargin => 0f;

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var cartesianChart = (CartesianChartModel)chart;
            var x = cartesianChart.Dimensions[0][ScalesAt[0]];
            var y = cartesianChart.Dimensions[1][ScalesAt[1]];

            float[] uw = chart.Get2DUiUnitWidth(x, y);

            int xi = 0, yi = 1;
            if (chart.InvertXy)
            {
                xi = 1;
                yi = 0;
            }

            // ReSharper disable CompareOfFloatsByEqualityOperator
            double[] d = new[]
            {
                x.InternalMaxValue - x.InternalMinValue == 0
                    ? float.MaxValue
                    : x.InternalMaxValue - x.InternalMinValue,
                y.InternalMaxValue - y.InternalMinValue == 0
                    ? float.MaxValue
                    : y.InternalMaxValue - y.InternalMinValue
            };

            // ReSharper restore CompareOfFloatsByEqualityOperator
            float wp = (float)(cartesianChart.DrawAreaSize[0] / d[xi]);
            float hp = (float)(cartesianChart.DrawAreaSize[1] / d[yi]);

            float minW = context.Ranges[2][ScalesAt[2]][0];
            float maxW = context.Ranges[2][ScalesAt[2]][1];

            var timeLine = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.View.AnimationLine
            };

            float originalDuration = (float)timeLine.Duration.TotalMilliseconds;
            IEnumerable<KeyFrame> originalAnimationLine = timeLine.AnimationLine;
            int i = 0;

            foreach (ChartPoint<TModel, WeightedCoordinate, IColoredShape> current in GetPoints(chart.View))
            {
                float[] p = new[]
                {
                    chart.ScaleToUi(current.Coordinate[0][0], x) - uw[0] *.5f,
                    chart.ScaleToUi(current.Coordinate[1][0], y) - uw[1] *.5f
                };

                var vm = new HeatViewModel
                {
                    Rectangle = new RectangleD(p[xi], p[yi], wp, hp),
                    From = current.Shape?.Color ?? Color.FromArgb(0, 255, 255, 255),
                    To = ColorInterpolation(minW, maxW, current.Coordinate.Weight)
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
                current.InteractionArea = new RectangleInteractionArea(vm.Rectangle);
                i++;
            }
        }

        /// <inheritdoc />
        protected override void SetDefaultColors(ChartModel chart)
        {
            if (Gradient != null)
            {
                return;
            }

            var nextColor = chart.GetNextColor();

            _gradient = new List<GradientStop>
            {
                new GradientStop
                {
                    Color = nextColor.SetOpacity(DefaultFillOpacity),
                    Offset = 0
                },
                new GradientStop
                {
                    Color = nextColor,
                    Offset = 1
                }
            };
        }

        private Color ColorInterpolation(float min, float max, float current)
        {
            if (_gradient == null) throw new Exception($"It was not possible call {nameof(ColorInterpolation)}() method, gradient is not specified.");

            float currentOffset = (current - min) / (max - min);
            IEnumerator<GradientStop> enumerator = _gradient.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                throw new LiveChartsException(120, null);
            }

            var from = enumerator.Current;

            while (enumerator.MoveNext())
            {
                var to = enumerator.Current;

                if (currentOffset >= from.Offset && currentOffset <= to.Offset)
                {
                    enumerator.Dispose();

                    double p = from.Offset + (from.Offset - to.Offset) *
                            ((currentOffset - from.Offset) / (from.Offset - to.Offset));

                    return Color.FromArgb(
                        (int)Math.Round(from.Color.A + p * (to.Color.A - from.Color.A)),
                        (int)Math.Round(from.Color.R + p * (to.Color.R - from.Color.R)),
                        (int)Math.Round(from.Color.G + p * (to.Color.G - from.Color.G)),
                        (int)Math.Round(from.Color.B + p * (to.Color.B - from.Color.B)));
                }

                from = to;
            }

            throw new LiveChartsException(121, null);
        }

        private void DrawPointShape(
            ChartPoint<TModel, WeightedCoordinate, IColoredShape> current,
            TimeLine timeline,
            HeatViewModel vm)
        {
            // initialize shape
            if (current.Shape == null)
            {
                current.Shape = UIFactory.GetNewColoredShape(current.Chart.Model);
                current.Chart.Content.AddChild(current.Shape, true);
                current.Shape.Color = vm.From;
            }

            // map properties
            current.Shape.Left = vm.Rectangle.Left;
            current.Shape.Top = vm.Rectangle.Top;
            current.Shape.Width = vm.Rectangle.Width;
            current.Shape.Height = vm.Rectangle.Height;
            current.Shape.ZIndex = ZIndex;

            // animate
            current.Shape.Animate(timeline)
                .Property(
                    nameof(IColoredShape.Color),
                    current.Shape.Color,
                    vm.To)
                .Begin();
        }
    }
}
