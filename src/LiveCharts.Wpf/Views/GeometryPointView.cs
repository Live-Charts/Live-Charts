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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction.Points;
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
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="Views.PointView{TModel, TPoint, WeightedCoordinate, GeometryPointViewModel, Path}" />
    public class GeometryPointView<TModel, TCoordinate, TSeries>
        : PointView<TModel, TCoordinate, GeometryPointViewModel, TSeries, Path>
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

            var r = vm.Diameter * .5;

            if (isNew)
            {
                Shape = new Path{Stretch = Stretch.Fill};
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, vm.Location.X - r);
                Canvas.SetTop(Shape, vm.Location.Y - r);
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

            if (isNew)
            {
                const BounceMagnitude bm = BounceMagnitude.Small;

                Shape.Animate()
                    .AtSpeed(speed)
                    .Bounce(Canvas.LeftProperty, vm.Location.X, vm.Location.X - r, bm)
                    .Bounce(Canvas.TopProperty, vm.Location.Y, vm.Location.Y - r, bm)
                    .Bounce(FrameworkElement.WidthProperty, 0, vm.Diameter, bm)
                    .Bounce(FrameworkElement.HeightProperty, 0, vm.Diameter, bm)
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

        /// <inheritdoc />
        protected override string GetLabelContent(
            Point<TModel, TCoordinate, GeometryPointViewModel, TSeries> point)
        {
            var chart = point.Chart;
            var xAxis = chart.Dimensions[0][point.Series.ScalesAt[0]];
            var yAxis = chart.Dimensions[1][point.Series.ScalesAt[1]];

            return $"{xAxis.FormatValue(point.Coordinate[0][0])}, {yAxis.FormatValue(point.Coordinate[1][0])}";
        }

        /// <inheritdoc />
        protected override void PlaceLabel(
            Point<TModel, TCoordinate, GeometryPointViewModel, TSeries> point,
            SizeF labelSize)
        {
            Canvas.SetTop(Label, point.ViewModel.Location.Y);
            Canvas.SetLeft(Label, point.ViewModel.Location.X);
        }

        /// <inheritdoc />
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