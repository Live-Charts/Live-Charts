using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private static PathFigure _f;
        private static bool _isIn;
        private BezierSegment _segment;

        protected override void OnDraw(TPoint point, TPoint previous)
        {
            CreateSegment(point);
            var chart = point.Chart.View;
            var viewModel = point.ViewModel;
            var isNew = Shape == null;

            if (isNew)
            {
                var wpfChart = (CartesianChart)point.Chart.View;
                Shape = new Path();
                Shape.Stretch = Stretch.Fill;
                wpfChart.DrawArea.Children.Add(Shape);
                Canvas.SetLeft(Shape, point.ViewModel.Location.X);
                Canvas.SetTop(Shape, point.ViewModel.Location.Y);
                Shape.Width = 0;
                Shape.Height = 0;
                _segment = new BezierSegment(point.ViewModel.Point1.AsWpf(), point.ViewModel.Point2.AsWpf(),
                    point.ViewModel.Point3.AsWpf(), true);
                _f.Segments.Add(_segment);
            }

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.Data = Geometry.Parse(Core.Drawing.Svg.Geometry.Circle.Data);  //Geometry.Parse(viewModel.Geometry.Data);

            var speed = chart.AnimationsSpeed;

            Shape.Animate()
                .AtSpeed(speed)
                .Property(Canvas.LeftProperty, viewModel.Location.X)
                .Property(Canvas.TopProperty, viewModel.Location.Y)
                .Property(FrameworkElement.WidthProperty, viewModel.GeometrySize)
                .Property(FrameworkElement.HeightProperty, viewModel.GeometrySize)
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
            
        }

        private Path _p;

        private void CreateSegment(TPoint point)
        {
            if (!_isIn)
            {
                var wpfChart = (CartesianChart)point.Chart.View;
               _p = new Path();
                wpfChart.DrawArea.Children.Add(_p);
                _isIn = true;
                _p.StrokeThickness = point.Series.StrokeThickness;
                _p.Stroke = point.Series.Stroke.AsWpf();
                _f = new PathFigure
                {
                    StartPoint = new System.Windows.Point(point.ViewModel.Location.AsWpf().X, point.ViewModel.Location.AsWpf().Y),
                    Segments = new PathSegmentCollection
                    {
                        new BezierSegment(
                            point.ViewModel.Point1.AsWpf(), point.ViewModel.Point2.AsWpf(), point.ViewModel.Point3.AsWpf(), true)
                    }
                };

                _p.Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        _f
                    }
                };
            }
        }
    }
}