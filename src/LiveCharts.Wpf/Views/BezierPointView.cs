using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;
using Frame = LiveCharts.Wpf.Animations.Frame;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Wpf.Views
{
    public class BezierPointView<TModel, TPoint, TLabel>
        : PointView<TModel, TPoint, Core.Coordinates.Point, BezierViewModel, Path, TLabel>
        where TPoint : Point<TModel, Core.Coordinates.Point, BezierViewModel>, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        private BezierSegment _segment;
        private ICartesianPath _path;
        private BezierPointView<TModel, TPoint, TLabel> _previous;
        private BezierPointView<TModel, TPoint, TLabel> _next;
        private BezierViewModel _vm;

        private bool IsMiddlePoint => _next != null && _previous != null;
        private bool IsDisposing { get; set; }

        protected override void OnDraw(TPoint point, TPoint previous)
        {
            var chart = point.Chart.View;
            _vm = point.ViewModel;
            var isNew = Shape == null;
            var speed = chart.AnimationsSpeed;
            _previous = (BezierPointView<TModel, TPoint, TLabel>) previous?.View;
            if (_previous != null)
            {
                _previous._next = this;
            }

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
                        new Frame(0.5, _vm.Location.X),
                        new Frame(0.8, _vm.Location.X - .5 * (_vm.GeometrySize + bounce)),
                        new Frame(0.9, _vm.Location.X - .5 * (_vm.GeometrySize - bounce * .5)),
                        new Frame(1, point.ViewModel.Location.X - .5 * _vm.GeometrySize))
                    .Property(Canvas.TopProperty,
                        new Frame(0.5, _vm.Location.Y),
                        new Frame(0.8, _vm.Location.Y - .5 * (_vm.GeometrySize + bounce)),
                        new Frame(0.9, _vm.Location.Y - .5 * (_vm.GeometrySize - bounce * .5)),
                        new Frame(1, point.ViewModel.Location.Y - .5 * _vm.GeometrySize))
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

            Shape.StrokeThickness = 3.5;
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = Brushes.White;
            Shape.Data = Geometry.Parse(Core.Drawing.Svg.Geometry.Circle.Data); // Geometry.Parse(viewModel.Geometry.Data);

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
        
        protected override void OnDrawLabel(TPoint point, Point location)
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