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
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;
using Brushes = System.Windows.Media.Brushes;
using Geometry = System.Windows.Media.Geometry;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The bezier point view class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TLabel">The type of the label.</typeparam>
    /// <seealso cref="Views.PointView{TModel, TPoint, PointCoordinate, BezierViewModel, Path}" />
    public class BezierPointView<TModel, TLabel>
        : PointView<TModel, PointCoordinate, BezierViewModel, ILineSeries, Path>
        where TLabel : FrameworkElement, new()
    {
        private BezierPointView<TModel, TLabel> _next;
        private ICartesianPath _path;
        private BezierPointView<TModel, TLabel> _previous;
        private BezierSegment _segment;
        private BezierViewModel _vm;
        private TimeLine _lastTimeLine;

        public SelfDrawnPath Path => (SelfDrawnPath) _path;

        private bool IsMiddlePoint => _next != null && _previous != null;

        protected override void OnDraw(
            Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> point, 
            Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> previous,
            TimeLine timeLine)
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
                chart.Content.AddChild(Shape, true);
                Canvas.SetLeft(Shape, point.ViewModel.Location.X);
                Canvas.SetTop(Shape, point.ViewModel.Location.Y);
                Shape.Width = 0;
                Shape.Height = 0;
                _path = _vm.Path;
                _segment = (BezierSegment) _path.InsertSegment(_segment, _vm.Index, _vm.Point1, _vm.Point2, _vm.Point3);

                var geometryAnimation = new TimeLine
                {
                    AnimationLine = timeLine.AnimationLine,
                    Duration = TimeSpan.FromMilliseconds(speed.TotalMilliseconds * 2)
                };

                var r = _vm.GeometrySize * .5;

                Shape.Animate(geometryAnimation)
                    .Property(Canvas.LeftProperty, _vm.Location.X + r, _vm.Location.X, 0.5)
                    .Property(Canvas.TopProperty, _vm.Location.Y + r, _vm.Location.Y, 0.5)
                    .Property(FrameworkElement.WidthProperty, 0, _vm.GeometrySize, 0.5)
                    .Property(FrameworkElement.HeightProperty, 0, _vm.GeometrySize, 0.5)
                    .Begin();
            }

            Shape.StrokeThickness = point.Series.StrokeThickness;
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = Brushes.White;
            Shape.Data = Geometry.Parse(point.Series.Geometry.Data);
            Panel.SetZIndex(Shape, point.Series.ZIndex);

            if (!isNew)
            {
                Shape.Animate(timeLine)
                    .Property(Canvas.LeftProperty, Canvas.GetLeft(Shape), _vm.Location.X)
                    .Property(Canvas.TopProperty, Canvas.GetTop(Shape), _vm.Location.Y)
                    .Begin();
            }

            _segment.Animate(timeLine)
                .Property(BezierSegment.Point1Property, _segment.Point1, _vm.Point1.AsWpf())
                .Property(BezierSegment.Point2Property, _segment.Point2, _vm.Point2.AsWpf())
                .Property(BezierSegment.Point3Property, _segment.Point3, _vm.Point3.AsWpf())
                .Begin();

            _lastTimeLine = timeLine;
        }

        protected override void PlaceLabel(
            Point<TModel, PointCoordinate, BezierViewModel, ILineSeries> point, 
            SizeF labelSize)
        {
            Canvas.SetTop(Label, point.ViewModel.Location.Y);
            Canvas.SetLeft(Label, point.ViewModel.Location.X);
        }

        protected override void OnDispose(IChartView chart)
        {
            var t = _next ?? _previous ?? this;

            var shapeAnimation = Shape.Animate(_lastTimeLine)
                .Property(
                    Canvas.LeftProperty, Canvas.GetLeft(Shape), t._vm.Location.X - .5 * _vm.GeometrySize)
                .Property(
                    Canvas.TopProperty, Canvas.GetTop(Shape), t._vm.Location.Y - .5 * _vm.GeometrySize);

            shapeAnimation
                .Then((sender, args) =>
                {
                    chart.Content.RemoveChild(Shape, true);
                })
                .Begin();

            if (IsMiddlePoint)
            {
                _path.RemoveSegment(_segment);
                _segment = null;
                _path = null;
                _lastTimeLine = null;
            }
            else
            {
                var segmentAnimation = _segment.Animate(_lastTimeLine)
                    .Property(BezierSegment.Point1Property, _segment.Point1, t._vm.Point1.AsWpf())
                    .Property(BezierSegment.Point2Property, _segment.Point2, t._vm.Point1.AsWpf())
                    .Property(BezierSegment.Point3Property, _segment.Point3, t._vm.Point1.AsWpf());

                segmentAnimation
                    .Then((sender, args) =>
                    {
                        _path.RemoveSegment(_segment);
                        _segment = null;
                        _path = null;
                        _lastTimeLine = null;
                        segmentAnimation.Dispose();
                    })
                    .Begin();
            }
        }
    }
}