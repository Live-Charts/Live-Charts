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
using System.Drawing;
using LiveCharts.Animations;
using LiveCharts.Charts;
using LiveCharts.Coordinates;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction;
using LiveCharts.Interaction.Areas;
using LiveCharts.Interaction.Points;
using LiveCharts.Updating;
#if NET45 || NET46
using Brush = LiveCharts.Drawing.Brushes.Brush;
#endif

#endregion

namespace LiveCharts.DataSeries
{
    /// <summary>
    /// The Pie series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="DataSeries.Series{TModel, PieCoordinate, TPointShape}" />
    /// <seealso cref="IPieSeries" />
    public class PieSeries<TModel> :
        StrokeSeries<TModel, StackedPointCoordinate, ISlice>, IPieSeries
    {
        private double _pushOut;
        private double _cornerRadius;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSeries{TModel}"/> class.
        /// </summary>
        public PieSeries()
        {
            DataLabelFormatter = coordinate => Format.AsMetricNumber(coordinate.Value);
            TooltipFormatter = coordinate =>
                $"{Format.AsMetricNumber(coordinate.Value)} / {Format.AsMetricNumber(coordinate.TotalStack)}";
            Global.Settings.BuildFromTheme<IPieSeries>(this);
            Global.Settings.BuildFromTheme<ISeries<StackedPointCoordinate>>(this);
        }

        /// <inheritdoc />
        public double PushOut
        {
            get => _pushOut;
            set
            {
                _pushOut = value;
                OnPropertyChanged(nameof(PushOut));
            }
        }

        /// <inheritdoc />
        public double CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                OnPropertyChanged(nameof(CornerRadius));
            }
        }

        /// <inheritdoc />
        public override Type ThemeKey => typeof(IPieSeries);

        /// <inheritdoc />
        public override float[] DefaultPointWidth => new[] { 0f, 0f };

        /// <inheritdoc />
        public override float PointMargin => 0f;

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart, UpdateContext context)
        {
            var pieChart = (IPieChartView)chart.View;

            double maxPushOut = context.GetMaxPushOut();

            float innerRadius = (float)pieChart.InnerRadius;
            float outerDiameter = pieChart.ControlSize[0] < pieChart.ControlSize[1]
                ? pieChart.ControlSize[0]
                : pieChart.ControlSize[1];

            outerDiameter -= (float)maxPushOut * 2f;

            var centerPoint = new PointF(pieChart.ControlSize[0] / 2f, pieChart.ControlSize[1] / 2f);

            float startsAt = pieChart.StartingRotationAngle > 360f
                ? 360f
                : (pieChart.StartingRotationAngle < 0
                    ? 0f
                    : (float)pieChart.StartingRotationAngle);

            var animation = AnimatableArguments.BuildFrom(chart.View, this);
            int i = 0;

            foreach (ChartPoint<TModel, StackedPointCoordinate, ISlice> current in GetPoints(chart.View))
            {
                float range = current.Coordinate.To - current.Coordinate.From;

                float stack;

                unchecked
                {
                    stack = context.GetStack((int)current.Coordinate.Key, 0, true);
                }

                var vm = new PieViewModel
                {
                    To = new SliceViewModel
                    {
                        Wedge = range * 360f / stack,
                        InnerRadius = innerRadius,
                        OuterRadius = outerDiameter / 2,
                        Rotation = startsAt + current.Coordinate.From * 360f / stack
                    },
                    ChartCenter = centerPoint
                };

                if (DelayRule != DelayRules.None)
                {
                    animation.SetDelay(DelayRule, i / (double)PointsCount);
                }

                current.Coordinate.TotalStack = stack;

                DrawPointShape(current, animation, vm);

                if (DataLabels)
                {
                    DrawPointLabel(current);
                }

                Mapper.EvaluateModelDependentActions(current.Model, current.Shape, current);
                current.InteractionArea = new PolarInteractionArea(
                    vm.To.OuterRadius, innerRadius, vm.To.Rotation, vm.To.Rotation + vm.To.Wedge, centerPoint);
                i++;
            }
        }

        private void DrawPointShape(
           ChartPoint<TModel, StackedPointCoordinate, ISlice> current,
           AnimatableArguments animationArgs,
           PieViewModel vm)
        {
            var shape = current.Shape;
            var isNew = shape == null;

            if (shape == null)
            {
                current.Shape = UIFactory.GetNewSlice(current.Chart.Model);
                shape = current.Shape;
                current.Shape.FlushToCanvas(current.Chart.Canvas, true);
                shape.Left = current.Chart.Model.DrawAreaSize[0] / 2 - vm.To.OuterRadius;
                shape.Top = current.Chart.Model.DrawAreaSize[1] / 2 - vm.To.OuterRadius;
                shape.Rotation = 0f;
                shape.Wedge = 0f;
                shape.Width = vm.To.OuterRadius * 2;
                shape.Height = vm.To.OuterRadius * 2;
            }

            // map properties
            shape.StrokeDashArray = StrokeDashArray;
            shape.StrokeThickness = StrokeThickness;
            shape.InnerRadius = vm.To.InnerRadius;
            shape.Radius = vm.To.OuterRadius;
            shape.ForceAngle = true;
            shape.CornerRadius = CornerRadius;
            shape.PushOut = PushOut;
            shape.Stroke = Stroke;
            shape.Fill = Fill;
            shape.Paint();

            // animate

            var shapeAnimation = shape.Animate(animationArgs);

            if (isNew)
            {
                shape.Radius = vm.To.OuterRadius * .8;
                shapeAnimation
                    .Property(nameof(ISlice.Radius), vm.From.InnerRadius, vm.To.OuterRadius)
                    .Property(nameof(ISlice.Rotation), 0, vm.To.Rotation)
                    .Property(nameof(ISlice.Wedge), 0, vm.To.Wedge);
            }
            else
            {
                shapeAnimation
                    .Property(nameof(ISlice.Rotation), shape.Rotation, vm.To.Rotation)
                    .Property(nameof(ISlice.Wedge), shape.Wedge, vm.To.Wedge);
            }

            shapeAnimation.Begin();
        }
    }
}
