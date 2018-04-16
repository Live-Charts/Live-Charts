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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;
using Brushes = System.Windows.Media.Brushes;
using Frame = LiveCharts.Wpf.Animations.Frame;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The bezier point view class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TLabel">The type of the label.</typeparam>
    /// <seealso cref="Views.PointView{TModel, TPoint, PointCoordinate, BezierViewModel, Path, TLabel}" />
    public class BezierPointView<TModel, TLabel>
        : PointView<TModel, PointCoordinate, BezierViewModel, ILineSeries, Path, TLabel>
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        private BezierPointView<TModel, TLabel> _next;
        private ICartesianPath _path;
        private BezierPointView<TModel, TLabel> _previous;
        private BezierSegment _segment;
        private BezierViewModel _vm;

        private bool IsMiddlePoint => _next != null && _previous != null;
        private bool IsDisposing { get; set; }

        protected override void OnDraw(
            Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> point, 
            Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> previous)
        {
            var chart = point.Chart.View;
            _vm = point.ViewModel;
            var isNew = Shape == null;
            var speed = chart.AnimationsSpeed;
            _previous = (BezierPointView<TModel, TLabel>) previous?.View;
            if (_previous != null) _previous._next = this;

            if (isNew)
            {
                Shape = new Path {Stretch = Stretch.Fill};
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, point.ViewModel.Location.X - .5 * _vm.GeometrySize);
                Canvas.SetTop(Shape, point.ViewModel.Location.Y - .5 * _vm.GeometrySize);
                Shape.Width = 0;
                Shape.Height = 0;
                _path = _vm.Path;
                _segment = (BezierSegment) _path.InsertSegment(_segment, _vm.Index, _vm.Point1, _vm.Point2, _vm.Point3);

                var bounce = .3 * _vm.GeometrySize;

                Shape.Animate()
                    .AtSpeed(TimeSpan.FromMilliseconds(speed.TotalMilliseconds * 2))
                    .Property(Canvas.LeftProperty,
                        new []
                        {
                            new Frame(0.5, _vm.Location.X),
                            new Frame(0.8, _vm.Location.X - .5 * (_vm.GeometrySize + bounce)),
                            new Frame(0.9, _vm.Location.X - .5 * (_vm.GeometrySize - bounce * .5)),
                            new Frame(1, point.ViewModel.Location.X - .5 * _vm.GeometrySize)
                        })
                    .Property(Canvas.TopProperty,
                        new []
                        {
                            new Frame(0.5, _vm.Location.Y),
                            new Frame(0.8, _vm.Location.Y - .5 * (_vm.GeometrySize + bounce)),
                            new Frame(0.9, _vm.Location.Y - .5 * (_vm.GeometrySize - bounce * .5)),
                            new Frame(1, point.ViewModel.Location.Y - .5 * _vm.GeometrySize)
                        })
                    .Properties(new[]
                        {
                            FrameworkElement.WidthProperty,
                            FrameworkElement.HeightProperty
                        },
                        new Frame(0.5, 0),
                        new Frame(0.8, _vm.GeometrySize + bounce),
                        new Frame(0.9, _vm.GeometrySize - bounce * .5),
                        new Frame(1, _vm.GeometrySize))
                    .Begin();
            }

            Shape.StrokeThickness = point.Series.StrokeThickness;
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = Brushes.White;
            Shape.Data = Geometry.Parse(point.Series.Geometry.Data);
            Panel.SetZIndex(Shape, point.Series.ZIndex);

            if (!isNew)
            {
                Shape.Animate()
                    .AtSpeed(speed)
                    .Property(Canvas.LeftProperty, _vm.Location.X - .5 * _vm.GeometrySize)
                    .Property(Canvas.TopProperty, _vm.Location.Y - .5 * _vm.GeometrySize)
                    .Begin();
            }

            _segment.Animate()
                .AtSpeed(speed)
                .Property(BezierSegment.Point1Property, _vm.Point1.AsWpf())
                .Property(BezierSegment.Point2Property, _vm.Point2.AsWpf())
                .Property(BezierSegment.Point3Property, _vm.Point3.AsWpf())
                .Begin();
        }

        protected override void OnDrawLabel(Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> point, PointF location)
        {
            base.OnDrawLabel(point, location);
        }

        protected override void OnDispose(IChartView chart)
        {
            IsDisposing = true;
            var t = _next ?? _previous ?? this;

            var shapeAnimation = Shape.Animate()
                .AtSpeed(chart.AnimationsSpeed)
                .Property(Canvas.LeftProperty, t._vm.Location.X - .5 * _vm.GeometrySize)
                .Property(Canvas.TopProperty, t._vm.Location.Y - .5 * _vm.GeometrySize);

            shapeAnimation
                .Then((sender, args) =>
                {
                    chart.Content.RemoveChild(Shape);
                    IsDisposing = false;
                })
                .Begin();

            if (IsMiddlePoint)
            {
                _path.RemoveSegment(_segment);
                _segment = null;
                _path = null;
            }
            else
            {
                var segmentAnimation = _segment.Animate()
                    .AtSpeed(chart.AnimationsSpeed)
                    .Property(BezierSegment.Point1Property, t._vm.Point1.AsWpf())
                    .Property(BezierSegment.Point2Property, t._vm.Point1.AsWpf())
                    .Property(BezierSegment.Point3Property, t._vm.Point1.AsWpf());

                segmentAnimation
                    .Then((sender, args) =>
                    {
                        IsDisposing = false;
                        _path.RemoveSegment(_segment);
                        _segment = null;
                        _path = null;
                        segmentAnimation.Dispose();
                    })
                    .Begin();
            }
        }
    }
}