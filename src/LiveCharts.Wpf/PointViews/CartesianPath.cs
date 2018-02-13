using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;

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

        public void SetStyle(
            Point startPoint, System.Drawing.Color stroke, System.Drawing.Color fill, 
            double strokeThickness, IEnumerable<double> strokeDashArray)
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

        public void Close(double length)
        {
            var l = length / _path.StrokeThickness;
            _path.StrokeDashArray = new DoubleCollection(new[] {l, l});
            _path.StrokeDashOffset = l;
            _path.BeginAnimation(
                Shape.StrokeDashOffsetProperty,
                new DoubleAnimation(l, 0, TimeSpan.FromMilliseconds(2000), FillBehavior.Stop));
            _path.StrokeDashOffset = 0;
        }

        public void Dispose(IChartView view)
        {
            var chart = (CartesianChart) view;
            chart.DrawArea.Children.Remove(_path);
        }
    }

    public static class UglyHelper
    {
        public static double GetLength(this Geometry geo)
        {
            PathGeometry path = geo.GetFlattenedPathGeometry();

            double length = 0.0;

            foreach (PathFigure pf in path.Figures)
            {
                System.Windows.Point start = pf.StartPoint;

                foreach (PolyLineSegment seg in pf.Segments)
                {
                    foreach (System.Windows.Point point in seg.Points)
                    {
                        length += Distance(start, point);
                        start = point;
                    }
                }
            }

            return length;
        }

        private static double Distance(System.Windows.Point p1, System.Windows.Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
