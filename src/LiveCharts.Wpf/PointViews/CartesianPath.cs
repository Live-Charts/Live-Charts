using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Assets.Models;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf.PointViews
{
    public class CartesianPath : ICartesianPath
    {
        private readonly Path _strokePath;
        private readonly PathFigure _figure;
        private IEnumerable<double> _strokeDashArray;

        public CartesianPath()
        {
            _strokePath = new Path();
            _figure = new PathFigure
            {
                Segments = new PathSegmentCollection()
            };
            _strokePath.Data = new PathGeometry
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
            chart.DrawArea.Children.Add(_strokePath);
        }

        public void SetStyle(
            Point startPoint, System.Drawing.Color stroke, System.Drawing.Color fill, 
            double strokeThickness, IEnumerable<double> strokeDashArray)
        {
            _figure.StartPoint = startPoint.AsWpf();
            _strokePath.Stroke = stroke.AsWpf();
            _strokePath.Fill = null;
            _strokePath.StrokeThickness = strokeThickness;

            _strokePath.StrokeThickness = 10;
            _strokePath.StrokeDashArray = new DoubleCollection(new[] {10d, 2,2});
            _strokeDashArray = strokeDashArray;
            _strokePath.StrokeDashOffset = 0;
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

        public void Close(IChartView view, double length)
        {
            var chart = (CartesianChart)view;
            var series = (LineSeries<City>) chart.Series.First();
            var first = series.Points.First();
            var last = series.Points.Last();
            var d = Math.Sqrt(
                Math.Pow(first.ViewModel.Location.X - last.ViewModel.Location.X, 2) +
                Math.Pow(first.ViewModel.Location.Y - last.ViewModel.Location.Y, 2));

            var l = length / _strokePath.StrokeThickness;
            _strokePath.StrokeDashArray = new DoubleCollection(GetAnimatedStrokeDashArray(l));
            _strokePath.StrokeDashOffset = l;
            _strokePath.BeginAnimation(
                Shape.StrokeDashOffsetProperty,
                new DoubleAnimation(l, 0, TimeSpan.FromMilliseconds(3000), FillBehavior.Stop));
            _strokePath.StrokeDashOffset = 0;
        }

        public void Dispose(IChartView view)
        {
            var chart = (CartesianChart) view;
            chart.DrawArea.Children.Remove(_strokePath);
        }

        private IEnumerable<double> GetAnimatedStrokeDashArray(double lenght)
        {
            var stack = 0d;
            var e = _strokeDashArray.GetEnumerator();
            var isStroked = true;
            while (stack < lenght)
            {
                if (!e.MoveNext())
                {
                    e.Reset();
                    e.MoveNext();
                }
                isStroked = !isStroked;
                yield return e.Current;
                stack += e.Current;
            }

            if (isStroked)
            {
                yield return 0;
            }
            yield return lenght;
            e.Dispose();
        }
    }
}
