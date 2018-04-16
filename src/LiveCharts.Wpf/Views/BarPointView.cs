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

using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Interaction;
using Orientation = LiveCharts.Core.Abstractions.Orientation;
using Rectangle = System.Windows.Shapes.Rectangle;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The column point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TShape">the type of the shape.</typeparam>
    /// <typeparam name="TLabel">the type of the label.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TSeries">The type fo the series.</typeparam>
    /// <seealso cref="PointView{TModel, Point,Point2D, ColumnViewModel, TShape, TLabel}" />
    public class BarPointView<TModel, TCoordinate, TSeries, TShape, TLabel>
        : PointView<TModel, TCoordinate, BarViewModel, TSeries, TShape, TLabel>
        where TCoordinate : ICoordinate
        where TSeries : IStrokeSeries
        where TShape : Shape, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        private Point<TModel, TCoordinate, BarViewModel, TSeries> _point;

        /// <inheritdoc />
        protected override void OnDraw(
            Point<TModel, TCoordinate, BarViewModel, TSeries> point, Point<TModel, TCoordinate, BarViewModel, TSeries> previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new TShape();
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, vm.From.Left);
                Canvas.SetTop(Shape, vm.From.Top);
                Shape.Width = vm.From.Width;
                Shape.Height = vm.From.Height;
            }

            // map properties
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.StrokeThickness = point.Series.StrokeThickness;
            Panel.SetZIndex(Shape, ((ICartesianSeries) point.Series).ZIndex);
            if (point.Series.StrokeDashArray != null)
            {
                Shape.StrokeDashArray = new DoubleCollection(point.Series.StrokeDashArray);
            }
            
            // special case
            var r = Shape as Rectangle;
            if (r != null)
            {
                var radius = (vm.Orientation == Orientation.Horizontal ? vm.To.Width : vm.To.Height) * .4;
                r.RadiusY = radius;
                r.RadiusX = radius;
            }

            // animate

            var animation = Shape.Animate().AtSpeed(chart.AnimationsSpeed);

            if (!isNewShape)
            {
                animation
                    .Property(Canvas.LeftProperty, vm.To.Left)
                    .Property(FrameworkElement.WidthProperty, vm.To.Width)
                    .Property(Canvas.TopProperty, vm.To.Top)
                    .Property(FrameworkElement.HeightProperty, vm.To.Height);
            }
            else
            {
                if (vm.Orientation == Orientation.Horizontal)
                {
                    animation
                        .Property(Canvas.LeftProperty, vm.To.Left)
                        .Property(FrameworkElement.WidthProperty, vm.To.Width)
                        .Bounce(Canvas.TopProperty, vm.From.Top, vm.To.Top)
                        .Bounce(FrameworkElement.HeightProperty, vm.From.Height, vm.To.Height);
                }
                else
                {
                    animation
                        .Property(Canvas.TopProperty, vm.To.Top)
                        .Property(FrameworkElement.HeightProperty, vm.To.Height)
                        .Bounce(Canvas.LeftProperty, vm.From.Left, vm.To.Left)
                        .Bounce(FrameworkElement.WidthProperty, vm.From.Width, vm.To.Width);
                }
            }

            animation.Begin();

            _point = point;
        }

        /// <inheritdoc />
        protected override void OnDrawLabel(Point<TModel, TCoordinate, BarViewModel, TSeries> point, PointF location)
        {
            var chart = point.Chart.View;
            var isNew = Label == null;

            if (isNew)
            {
                Label = new TLabel();
                Label.Measure(point.PackAll());
                Canvas.SetLeft(Shape, Canvas.GetLeft(Shape));
                Canvas.SetTop(Shape, Canvas.GetTop(Shape));
                chart.Content.AddChild(Label);
            }

            var speed = chart.AnimationsSpeed;

            Label.BeginAnimation(
                Canvas.LeftProperty,
                new DoubleAnimation(location.X, speed));
            Label.BeginAnimation(
                Canvas.TopProperty,
                new DoubleAnimation(location.Y, speed));
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart)
        {
            var cartesianSeries = (ICartesianSeries) _point.Series;
            var zero = chart.Model.ScaleToUi(0, chart.Dimensions[1][cartesianSeries.ScalesAt[1]]);

            var animation = Shape.Animate()
                .AtSpeed(chart.AnimationsSpeed)
                .Property(Canvas.TopProperty, zero)
                .Property(FrameworkElement.HeightProperty, 0);

            animation.Then((sender, args) =>
            {
                chart.Content.RemoveChild(chart);
                chart.Content.RemoveChild(Label);
                animation.Dispose();
                animation = null;
            }).Begin();
        }
    }
}