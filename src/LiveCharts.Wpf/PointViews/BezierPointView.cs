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
        private static PathFigure _f;
        private static bool _isIn;

        protected override void OnDraw(TPoint point, TPoint previous)
        {
            if (!_isIn)
            {
                var wpfChart = (CartesianChart) point.Chart.View;
                var p = new Path();
                wpfChart.DrawArea.Children.Add(p);
                _isIn = true;
                p.StrokeThickness = point.Series.StrokeThickness;
                p.Stroke = point.Series.Stroke.AsWpf();
                _f = new PathFigure
                {
                    StartPoint = new System.Windows.Point(point.ViewModel.Location.AsWpf().X, point.ViewModel.Location.AsWpf().Y),
                    Segments = new PathSegmentCollection
                    {
                        new BezierSegment(
                            point.ViewModel.Point1.AsWpf(), point.ViewModel.Point2.AsWpf(), point.ViewModel.Point3.AsWpf(), true)
                    }
                };

                p.Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                       _f
                    }
                };
            }

            var isNew = Shape == null;

            if (isNew)
            {
                var wpfChart = (CartesianChart)point.Chart.View;
                Shape = new Path();
                Shape.Stretch = Stretch.Fill;
                wpfChart.DrawArea.Children.Add(Shape);
                Canvas.SetLeft(Shape, point.ViewModel.Location.X);
                Canvas.SetTop(Shape, point.ViewModel.Location.Y);

                _f.Segments.Add(new BezierSegment(point.ViewModel.Point1.AsWpf(), point.ViewModel.Point2.AsWpf(), point.ViewModel.Point3.AsWpf(), true));
            }

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.Data = Geometry.Parse(Core.Drawing.Svg.Geometry.Circle.Data);  //Geometry.Parse(viewModel.Geometry.Data);
            Shape.Width = point.ViewModel.GeometrySize;
            Shape.Height = point.ViewModel.GeometrySize;
        }
        
        protected override void OnDrawLabel(TPoint point, Point location)
        {
            base.OnDrawLabel(point, location);
        }

        protected override void OnDispose(IChartView chart)
        {
            base.OnDispose(chart);
        }
    }
}