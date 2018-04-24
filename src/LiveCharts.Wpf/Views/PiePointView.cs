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
        /// <inheritdoc />
        protected override void OnDraw(
            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> point,
            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new Slice();
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, point.Chart.DrawAreaSize[0] / 2 - vm.To.OuterRadius);
                Canvas.SetTop(Shape, point.Chart.DrawAreaSize[1] / 2 - vm.To.OuterRadius);
                Shape.Rotation = 0d;
                Shape.Wedge = 0d;
                Shape.Width = vm.To.OuterRadius * 2;
                Shape.Height = vm.To.OuterRadius * 2;
            }

            // map properties
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.StrokeThickness = point.Series.StrokeThickness;
            if (point.Series.StrokeDashArray != null)
            {
                Shape.StrokeDashArray = new DoubleCollection(point.Series.StrokeDashArray);
            }
            Shape.InnerRadius = vm.To.InnerRadius;
            Shape.Radius = vm.To.OuterRadius;
            Shape.ForceAngle = true;
            Shape.CornerRadius = point.Series.CornerRadius;
            Shape.PushOut = point.Series.PushOut;

            // animate

            var animation = Shape.Animate().AtSpeed(chart.AnimationsSpeed);

            if (isNewShape)
            {
                Shape.Radius = vm.To.OuterRadius * .8;
                animation
                    .Property(Slice.RadiusProperty, vm.To.OuterRadius, 0)
                    .Property(Slice.RotationProperty, vm.To.Rotation)
                    .Bounce(Slice.WedgeProperty, 0, vm.To.Wedge);
            }
            else
            {
                animation
                    .Property(Slice.RotationProperty, vm.To.Rotation)
                    .Property(Slice.WedgeProperty, vm.To.Wedge);
            }

            animation.Begin();
        }

        /// <inheritdoc />
        protected override string GetLabelContent(
            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> point)
        {
            var valueAxis = point.Chart.Dimensions[1][0];
            return valueAxis.FormatValue(point.Coordinate.Value);
        }

        /// <inheritdoc />
        protected override void PlaceLabel(
            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> point,
            SizeF labelSize)
        {
            // this one could be a complex task...
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart)
        {
            base.OnDispose(chart);
        }
    }
}