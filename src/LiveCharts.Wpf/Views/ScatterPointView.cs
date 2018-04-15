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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// Geometry point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TLabel">The type of the label.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="Views.PointView{TModel, TPoint, WeightedCoordinate, GeometryPointViewModel, Path, TLabel}" />
    public class GeometryPointView<TModel, TCoordinate, TSeries, TLabel>
        : PointView<TModel, TCoordinate, GeometryPointViewModel, TSeries, Path, TLabel>
        where TLabel : FrameworkElement, IDataLabelControl, new()
        where TCoordinate : ICoordinate
        where TSeries : IStrokeSeries, ICartesianSeries
    {
        /// <inheritdoc />
        protected override void OnDraw(
            Point<TModel, TCoordinate, GeometryPointViewModel, TSeries> point,
            Point<TModel, TCoordinate, GeometryPointViewModel, TSeries> previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNew = Shape == null;

            if (isNew)
            {
                Shape = new Path{Stretch = Stretch.Fill};
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, vm.Location.X);
                Canvas.SetTop(Shape, vm.Location.Y);
                Shape.Width = 0;
                Shape.Height = 0;
            }

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.StrokeThickness = 3.5;
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Data = Geometry.Parse(point.Series.Geometry.Data);
            Panel.SetZIndex(Shape, point.Series.ZIndex);

            var speed = chart.AnimationsSpeed;
            var r = vm.Diameter * .5;
            var b = vm.Diameter * .18;

            if (isNew)
            {
                Shape.Animate()
                    .AtSpeed(speed)
                    .InverseBounce(Canvas.LeftProperty, vm.Location.X - r, b * .5)
                    .InverseBounce(Canvas.TopProperty, vm.Location.Y - r, b * .5)
                    .Bounce(FrameworkElement.WidthProperty, vm.Diameter, b)
                    .Bounce(FrameworkElement.HeightProperty, vm.Diameter, b)
                    .Begin();
            }
            else
            {
                Shape.Animate()
                    .AtSpeed(speed)
                    .Property(Canvas.LeftProperty, vm.Location.X - r)
                    .Property(Canvas.TopProperty, vm.Location.Y - r)
                    .Begin();
            }
        }

        protected override void OnDispose(IChartView chart)
        {
            var animation = Shape.Animate()
                .AtSpeed(chart.AnimationsSpeed)
                .Property(FrameworkElement.HeightProperty, 0)
                .Property(FrameworkElement.WidthProperty, 0);

            animation.Then((sender, args) =>
            {
                chart.Content.RemoveChild(Shape);
                chart.Content.RemoveChild(Label);
                animation.Dispose();
                animation = null;
            }).Begin();
        }
    }
}