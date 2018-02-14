using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Wpf.PointViews
{
    public class BezierPointView<TModel, TPoint, TCoordinate, TViewModel, TLabel>
        : PointView<TModel, TPoint, TCoordinate, TViewModel, Path, TLabel>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : Point2D
        where TViewModel : BezierViewModel
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        private BezierSegment _segment;
        private ICartesianPath _path;

        protected override void OnDraw(TPoint point, TPoint previous)
        {
            var chart = point.Chart.View;
            var viewModel = point.ViewModel;
            var isNew = Shape == null;
            var speed = chart.AnimationsSpeed;

            if (isNew)
            {
                var wpfChart = (CartesianChart)point.Chart.View;
                Shape = new Path {Stretch = Stretch.Fill};
                wpfChart.DrawArea.Children.Add(Shape);
                Canvas.SetLeft(Shape, point.ViewModel.Location.X - .5 * viewModel.GeometrySize);
                Canvas.SetTop(Shape, point.ViewModel.Location.Y - .5 * viewModel.GeometrySize);
                Shape.Width = 0;
                Shape.Height = 0;
                _path = viewModel.Path;
                _segment = (BezierSegment) _path.AddBezierSegment(
                    viewModel.Point1, viewModel.Point2, viewModel.Point3);

                var bounce = .3 * viewModel.GeometrySize;

                Shape.Animate()
                    .AtSpeed(speed)
                    .Property(FrameworkElement.WidthProperty,
                        new Frame(0.5, 0),
                        new Frame(0.8, viewModel.GeometrySize + bounce),
                        new Frame(0.9, viewModel.GeometrySize - bounce * .5),
                        new Frame(1, viewModel.GeometrySize))
                    .Property(FrameworkElement.HeightProperty,
                        new Frame(0.5, 0),
                        new Frame(0.8, viewModel.GeometrySize + bounce),
                        new Frame(0.9, viewModel.GeometrySize - bounce * .5),
                        new Frame(1, viewModel.GeometrySize))
                    .Begin();
            }

            Shape.StrokeThickness = 3.5;
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = Brushes.White;
            Shape.Data = Geometry.Parse(Core.Drawing.Svg.Geometry.Circle.Data); // Geometry.Parse(viewModel.Geometry.Data);

            Shape.Animate()
                .AtSpeed(speed)
                .Property(Canvas.LeftProperty, viewModel.Location.X - .5 * viewModel.GeometrySize)
                .Property(Canvas.TopProperty, viewModel.Location.Y - .5 * viewModel.GeometrySize)
                .Begin();

            _segment.Animate()
                .AtSpeed(speed)
                .Property(BezierSegment.Point1Property, point.ViewModel.Point1.AsWpf())
                .Property(BezierSegment.Point2Property, point.ViewModel.Point2.AsWpf())
                .Property(BezierSegment.Point3Property, point.ViewModel.Point3.AsWpf())
                .Begin();
        }
        
        protected override void OnDrawLabel(TPoint point, Point location)
        {
            base.OnDrawLabel(point, location);
        }

        protected override void OnDispose(IChartView chart)
        {
            var wpfChart = (CartesianChart) chart;
            wpfChart.DrawArea.Children.Remove(Shape);
            _path.RemoveSegment(_segment);
            _segment = null;
            _path = null;
        }
    }
}