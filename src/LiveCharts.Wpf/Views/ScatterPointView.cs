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
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;

#endregion

namespace LiveCharts.Wpf.Views
{
    public class ScatterPointView<TModel, TPoint, TLabel>
        : PointView<TModel, TPoint, WeightedPoint, ScatterViewModel, Path, TLabel>
        where TPoint : Point<TModel, WeightedPoint, ScatterViewModel>, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        protected override void OnDraw(TPoint point, TPoint previous)
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
            Shape.Data = Geometry.Parse(Core.Drawing.Svg.Geometry.Circle.Data); // Geometry.Parse(viewModel.Geometry.Data);

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