﻿#region License
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
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;
using Geometry = System.Windows.Media.Geometry;

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
        private TimeLine _lastTimeLine;

        /// <inheritdoc />
        protected override void OnDraw(
            ChartPoint<TModel, TCoordinate, GeometryPointViewModel, TSeries> chartPoint,
            ChartPoint<TModel, TCoordinate, GeometryPointViewModel, TSeries> previous,
            TimeLine timeLine)
        {
            var chart = chartPoint.Chart;
            var vm = chartPoint.ViewModel;
            bool isNew = Shape == null;

            if (isNew)
            {
                Shape = new Path{Stretch = Stretch.Fill};
                chart.Content.AddChild(Shape, true);
                Canvas.SetLeft(Shape, vm.Location.X);
                Canvas.SetTop(Shape, vm.Location.Y);
                Shape.Width = 0;
                Shape.Height = 0;
            }

            Shape.Stroke = chartPoint.Series.Stroke.AsWpf();
            Shape.Fill = chartPoint.Series.Fill.AsWpf();
            Shape.StrokeThickness = chartPoint.Series.StrokeThickness;
            Shape.Stroke = chartPoint.Series.Stroke.AsWpf();
            Shape.Data = Geometry.Parse(chartPoint.Series.Geometry.Data);
            Panel.SetZIndex(Shape, chartPoint.Series.ZIndex);

            float r = vm.Diameter * .5f;

            if (isNew)
            {
                Shape.Animate(timeLine)
                    .Property(Canvas.LeftProperty, vm.Location.X + r, vm.Location.X)
                    .Property(Canvas.TopProperty, vm.Location.Y + r, vm.Location.Y)
                    .Property(FrameworkElement.WidthProperty, 0, vm.Diameter)
                    .Property(FrameworkElement.HeightProperty, 0, vm.Diameter)
                    .Begin();
            }
            else
            {
                Shape.Animate(timeLine)
                    .Property(Canvas.LeftProperty, Canvas.GetLeft(Shape), vm.Location.X)
                    .Property(Canvas.TopProperty, Canvas.GetTop(Shape), vm.Location.Y)
                    .Begin();
            }

            _lastTimeLine = timeLine;
        }

        /// <inheritdoc />
        protected override void PlaceLabel(
            ChartPoint<TModel, TCoordinate, GeometryPointViewModel, TSeries> chartPoint,
            SizeF labelSize)
        {
            Canvas.SetTop(Label, chartPoint.ViewModel.Location.Y);
            Canvas.SetLeft(Label, chartPoint.ViewModel.Location.X);
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

            var animation = Shape.Animate(_lastTimeLine)
                .Property(FrameworkElement.HeightProperty, Shape.Height, 0)
                .Property(FrameworkElement.WidthProperty, Shape.Width, 0);

            animation.Then((sender, args) =>
            {
                chart.Content?.DisposeChild(Shape, true);
                chart.Content?.DisposeChild(Label, true);
                animation.Dispose();
                animation = null;
                _lastTimeLine = null;
            }).Begin();
        }
    }
}