using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;
using LiveCharts.Wpf.Animations;

namespace LiveCharts.Wpf.PointViews
{
    public class CartesianPath : ICartesianPath
    {
        private readonly Path _strokePath;
        private readonly PathFigure _figure;
        private IEnumerable<double> _strokeDashArray;
        private double _previousLenght;
        private CartesianChart _chart;

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

        /// <inheritdoc />
        public void Initialize(IChartView view)
        {
            _chart = (CartesianChart) view;
            _chart.DrawArea.Children.Add(_strokePath);
        }

        /// <inheritdoc />
        public void SetStyle(
            Point startPoint, System.Drawing.Color stroke, System.Drawing.Color fill, 
            double strokeThickness, IEnumerable<double> strokeDashArray)
        {
            _figure.Animate()
                .AtSpeed(_chart.AnimationsSpeed)
                .Property(PathFigure.StartPointProperty, startPoint.AsWpf())
                .Begin();
            _strokePath.Stroke = stroke.AsWpf();
            _strokePath.Fill = null;
            _strokePath.StrokeThickness = strokeThickness;
            _strokeDashArray = strokeDashArray;
            _strokePath.StrokeDashOffset = 0;
        }

        /// <inheritdoc />
        public object InsertSegment(object segment, int index, Point p1, Point p2, Point p3)
        {
            var s = (BezierSegment) segment ?? new BezierSegment(p1.AsWpf(), p2.AsWpf(), p3.AsWpf(), true);

            _figure.Segments.Remove(s);
            _figure.Segments.Insert(index, s);

            return s;
        }

        public void RemoveSegment(object segment)
        {
            _figure.Segments.Remove((PathSegment) segment);
        }

        public void Close(IChartView view, double length, double i, double j)
        {
            var chart = (CartesianChart) view;

            var l = length / _strokePath.StrokeThickness;
            var tl = l - _previousLenght;
            var remaining = 0d;
            if (tl < 0)
            {
                remaining = -tl;
            }

            _strokePath.StrokeDashArray = new DoubleCollection(GetAnimatedStrokeDashArray(l+remaining));
            _strokePath.BeginAnimation(
                Shape.StrokeDashOffsetProperty,
                new DoubleAnimation(tl + remaining, 0, chart.AnimationsSpeed, FillBehavior.Stop));

            _strokePath.StrokeDashOffset = 0;
            _previousLenght = l;
        }

        public void Dispose(IChartView view)
        {
            _chart.DrawArea.Children.Remove(_strokePath);
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
