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

using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;
using Slice = LiveCharts.Wpf.Shapes.Slice;
using TimeLine = LiveCharts.Core.Animations.TimeLine;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The pie point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Wpf.Views.PointView{TModel, TPoint, PieCoordinate, PieViewModel, TShape}" />
    public class PiePointView<TModel>
        : PointView<TModel, StackedPointCoordinate, PieViewModel, IPieSeries, Slice>
    {
        private TimeLine _lastTimeLine;

        /// <inheritdoc />
        protected override void OnDraw(
            ChartPoint<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> chartPoint,
            ChartPoint<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> previous,
            TimeLine timeLine)
        {
            var chart = chartPoint.Chart;
            var vm = chartPoint.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new Slice();
                chart.Content.AddChild(Shape, true);
                Canvas.SetLeft(Shape, chartPoint.Chart.Model.DrawAreaSize[0] / 2 - vm.To.OuterRadius);
                Canvas.SetTop(Shape, chartPoint.Chart.Model.DrawAreaSize[1] / 2 - vm.To.OuterRadius);
                Shape.Rotation = 0d;
                Shape.Wedge = 0d;
                Shape.Width = vm.To.OuterRadius * 2;
                Shape.Height = vm.To.OuterRadius * 2;
            }

            // map properties
            Shape.Stroke = chartPoint.Series.Stroke.AsWpf();
            Shape.Fill = chartPoint.Series.Fill.AsWpf();
            Shape.StrokeThickness = chartPoint.Series.StrokeThickness;
            if (chartPoint.Series.StrokeDashArray != null)
            {
                Shape.StrokeDashArray = new DoubleCollection(chartPoint.Series.StrokeDashArray);
            }
            Shape.InnerRadius = vm.To.InnerRadius;
            Shape.Radius = vm.To.OuterRadius;
            Shape.ForceAngle = true;
            Shape.CornerRadius = chartPoint.Series.CornerRadius;
            Shape.PushOut = chartPoint.Series.PushOut;

            // animate

            var shapeAnimation = Shape.Animate(timeLine);

            if (isNewShape)
            {
                Shape.Radius = vm.To.OuterRadius * .8;
                shapeAnimation
                    .Property(Slice.RadiusProperty, vm.From.InnerRadius, vm.To.OuterRadius)
                    .Property(Slice.RotationProperty, 0, vm.To.Rotation)
                    .Property(Slice.WedgeProperty, 0, vm.To.Wedge);
            }
            else
            {
                shapeAnimation
                    .Property(Slice.RotationProperty, Shape.Rotation, vm.To.Rotation)
                    .Property(Slice.WedgeProperty, Shape.Wedge, vm.To.Wedge);
            }

            shapeAnimation.Begin();
            _lastTimeLine = timeLine;
        }

        /// <inheritdoc />
        protected override void PlaceLabel(
            ChartPoint<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> chartPoint,
            SizeF labelSize)
        {
            // this one could be a complex task...
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart, bool force)
        {
            if (force)
            {
                chart.Content.DisposeChild(Shape, true);
                chart.Content.DisposeChild(Label, true);
                _lastTimeLine = null;
                return;
            }

            var sliceAnimation = Shape.Animate(_lastTimeLine)
                .Property(Slice.WedgeProperty, Shape.Wedge, 0);

            sliceAnimation
                .Then((sender, args) =>
                {
                    chart.Content?.DisposeChild(Shape, true);
                    chart.Content?.DisposeChild(Label, true);
                    sliceAnimation.Dispose();
                    sliceAnimation = null;
                    _lastTimeLine = null;
                });
        }
    }
}