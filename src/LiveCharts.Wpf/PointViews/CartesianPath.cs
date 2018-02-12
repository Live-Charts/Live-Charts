using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;
using Color = LiveCharts.Core.Drawing.Color;

namespace LiveCharts.Wpf.PointViews
{
    public class CartesianPath : ICartesianPath
    {
        private readonly Path _path;
        private readonly PathFigure _figure;

        public CartesianPath()
        {
            _path = new Path();
            _figure = new PathFigure
            {
                Segments = new PathSegmentCollection()
            };
            _path.Data = new PathGeometry
            {
                Figures = new PathFigureCollection(1)
                {
                    _figure
                }
            };
        }

        public void Initialize(IChartView view)
        {
            var chart = (CartesianChart) view;
            chart.DrawArea.Children.Add(_path);
        }

        public void SetStyle(Point startPoint, Color stroke, Color fill, double strokeThickness, IEnumerable<double> strokeDashArray)
        {
            _figure.StartPoint = startPoint.AsWpf();
            _path.Stroke = stroke.AsWpf();
            _path.Fill = fill.AsWpf();
            _path.StrokeThickness = strokeThickness;
            _path.StrokeDashArray = strokeDashArray == null ? null : new DoubleCollection(strokeDashArray);
            _path.StrokeDashOffset = 0;
        }

        public object AddBezierSegment(Point p1, Point p2, Point p3)
        {
            var segment = new BezierSegment(p1.AsWpf(), p2.AsWpf(), p3.AsWpf(), true);
            _figure.Segments.Add(segment);
            return segment;
        }

        public void RemoveSegment(object segment)
        {
            _figure.Segments.Remove((PathSegment) segment);
        }

        public void Close()
        {

        }

        public void Dispose(IChartView view)
        {
            var chart = (CartesianChart) view;
            chart.DrawArea.Children.Remove(_path);
        }
    }
}
