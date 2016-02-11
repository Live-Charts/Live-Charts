//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class LineSeries : Series
    {
        private TimeSpan d = TimeSpan.FromMilliseconds(500);
        private bool _isPrimitive;
        private Dictionary<int, Line> _primitiveDictionary = new Dictionary<int, Line>();
        private Dictionary<object, Line> _dictionary = new Dictionary<object, Line>();

        public LineSeries()
        {
            StrokeThickness = 2.5;
            PointRadius = 4;
        }

        private ILine LineChart
        {
            get { return Chart as ILine; }
        }

        public double StrokeThickness { get; set; }

        public double PointRadius { get; set; }

        public override void Plot(bool animate = true)
        {
            _isPrimitive = Values.Count >= 1 && Values[0].GetType().IsPrimitive;

            if (Visibility != Visibility.Visible) return;

            if(LineChart.LineType == LineChartLineType.Polyline)
                PlotLines();
            
        }

        private void PlotLines()
        {
            var rr = PointRadius < 2.5 ? 2.5 : PointRadius;
            var pCount = Values.Points.Count();

            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            foreach (var segment in Values.Points.AsSegments())
            {
                var point = segment[0];
                var l = GetLine(0, point.Instance);
                var p = new Point(ToDrawMargin(point.X, AxisTags.X), ToDrawMargin(point.Y, AxisTags.Y));
                if (l.IsNew)
                {
                    l.Line.X1 = p.X;
                    l.Line.Y1 = p.Y;
                }
                else
                {
                    l.Line.BeginAnimation(Line.X1Property, new DoubleAnimation(l.Line.X1, p.X, d));
                    l.Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(l.Line.Y1, p.Y, d));
                }
                
                var previousLine = l.Line;
                var isPreviousNew = false;

                for (int index = 1; index < segment.Count; index++)
                {
                    point = segment[index];
                    if (index != segment.Count -1) l = GetLine(index, point.Instance);
                    p = new Point(ToDrawMargin(point.X, AxisTags.X), ToDrawMargin(point.Y, AxisTags.Y));

                    var x = previousLine == null ? p.X : previousLine.X2;
                    var y = previousLine == null ? p.Y : previousLine.Y2;

                    if (previousLine != null)
                    {
                        if (index != segment.Count - 1)
                        {
                            var px = isPreviousNew ? p.X : previousLine.X2;
                            var py = isPreviousNew ? p.Y : previousLine.Y2;
                            l.Line.BeginAnimation(Line.X1Property, new DoubleAnimation(px, p.X, d));
                            l.Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(py, p.Y, d));
                        }
                        var cx = isPreviousNew ? p.X : x;
                        var cy = isPreviousNew ? p.Y : y;
                        previousLine.BeginAnimation(Line.X2Property, new DoubleAnimation(cx, p.X, d));
                        previousLine.BeginAnimation(Line.Y2Property, new DoubleAnimation(cy, p.Y, d));
                    }

                    previousLine = l.Line;
                    isPreviousNew = l.IsNew;

                    //if (DataLabels) AddDataLabel(p1, chrtP1, f);
                    //var mark = AddPointMarkup(p1);
                    //AddHoverAndClickShape(rr, p1, chrtP1, mark);
                }
            }
        }

        private static double GetCombination(int k, int n)
        {
            if (k == 0 || n == 0 || k == n)
            {
                return 1;
            }
            var a = 1d;
            var b = n;
            for (int i = 2; i < k; i++)
            {
                a *= i;
                b *= n - i;
            }
            return b / a;
        }

        private static double GetFactor(double t, int k, int n)
        {
            double result = GetCombination(k, n) * Math.Pow(t, k) * Math.Pow(1 - t, n - k);
            return result;
        }

        private static Point GetPoint(double t, Point[] points)
        {
            double x = 0;
            double y = 0;
            var n = points.Length;
            for (var i = 0; i < n; i++)
            {
                var factor = GetFactor(t, i, n - 1);
                x += points[i].X * factor;
                y += points[i].Y * factor;
            }
            var p = new Point(x, y);
            return p;
        }

        private static double GetBezierLength(Point[] points)
        {
            const int steps = 5;
            double length = 0;
            var last = GetPoint(0, points);
            for (var i = 1; i <= steps; i++)
            {
                var p = GetPoint((1.0 / steps) * i, points);
                var dx = p.X - last.X;
                var dy = p.Y - last.Y;
                length += Math.Sqrt(dx * dx + dy * dy);
                last = p;
            }
            return length;
        }

        private IEnumerable<Shape> _addSerieAsBezier(Point[] points, bool animate = true)
        {
            if (points.Length < 2) return Enumerable.Empty<Shape>();
            var addedFigures = new List<Shape>();

            Point[] cp1, cp2;
            BezierSpline.GetCurveControlPoints(points, out cp1, out cp2);

            var lines = new PathSegmentCollection();
            var areaLines = new PathSegmentCollection { new LineSegment(points[0], true) };
            var l = 0d;
            for (var i = 0; i < cp1.Length; ++i)
            {
                lines.Add(new BezierSegment(cp1[i], cp2[i], points[i + 1], true));
                areaLines.Add(new BezierSegment(cp1[i], cp2[i], points[i + 1], true));
                l += GetBezierLength(new [] { points[i], cp1[i], cp2[i], points[i + 1] });
            }
            l *= 1.05;
            l /= StrokeThickness;
            var lastP = Chart.Invert
                ? new Point(ToDrawMargin(Chart.Min.X, AxisTags.X), points.Min(x => x.Y))
                : new Point(points.Max(x => x.X), ToDrawMargin(Chart.Min.Y, AxisTags.Y));
            areaLines.Add(new LineSegment(lastP, true));
            var f = new PathFigure(points[0], lines, false);
            var aOrigin = Chart.Invert
                ? new Point(ToDrawMargin(Chart.Min.X, AxisTags.X), points.Max(x => x.Y))
                : new Point(points.Min(x => x.X), ToDrawMargin(Chart.Min.Y, AxisTags.Y));
            var fa = new PathFigure(aOrigin, areaLines, false);
            var g = new PathGeometry(new[] { f });
            var ga = new PathGeometry(new[] { fa });

            var path = new Path
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                Data = g,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeDashOffset = l,
                StrokeDashArray = new DoubleCollection { l, l },
                ClipToBounds = true
            };
            var patha = new Path
            {
                StrokeThickness = 0,
                Data = ga,
                Fill = Fill,
                ClipToBounds = true
            };

            Chart.DrawMargin.Children.Add(path);
            addedFigures.Add(path);

            Chart.DrawMargin.Children.Add(patha);
            addedFigures.Add(patha);

            var draw = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromSeconds(0),
                KeyFrames = new DoubleKeyFrameCollection
                {
                    new SplineDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromMilliseconds(1),
                        Value = l
                    },
                    new SplineDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromMilliseconds(750),
                        Value = 0
                    }
                }
            };

            Storyboard.SetTarget(draw, path);
            Storyboard.SetTargetProperty(draw, new PropertyPath(Shape.StrokeDashOffsetProperty));
            var sbDraw = new Storyboard();
            sbDraw.Children.Add(draw);
            var animated = false;
            if (!Chart.DisableAnimation)
            {
                if (animate)
                {
                    sbDraw.Begin();
                    animated = true;
                }
            }
            if (!animated) path.StrokeDashOffset = 0;
            return addedFigures;
        }

        private IEnumerable<Shape> _addSeriesAsPolyline(IList<Point> points, bool animate = true)
        {
            if (points.Count < 2) return Enumerable.Empty<Shape>();
            var addedFigures = new List<Shape>();

            var l = 0d;
            for (var i = 1; i < points.Count; i++)
            {
                var p1 = points[i - 1];
                var p2 = points[i];
                l += Math.Sqrt(
                    Math.Pow(Math.Abs(p1.X - p2.X), 2) +
                    Math.Pow(Math.Abs(p1.Y - p2.Y), 2)
                    );
            }

            var f = points.First();
            var p = points.Where(x => x != f);
            var g = new PathGeometry
            {
                Figures = new PathFigureCollection(new List<PathFigure>
                {
                    new PathFigure
                    {
                        StartPoint = f,
                        Segments = new PathSegmentCollection(
                            p.Select(x => new LineSegment {Point = new Point(x.X, x.Y)}))
                    }
                })
            };

            var path = new Path
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                Data = g,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeDashOffset = l,
                StrokeDashArray = new DoubleCollection { l, l },
                ClipToBounds = true
            };

            var sp = points.ToList();
            sp.Add(new Point(points.Max(x => x.X), ToDrawMargin(Chart.Min.Y, AxisTags.Y)));
            var sg = new PathGeometry
            {
                Figures = new PathFigureCollection(new List<PathFigure>
                {
                    new PathFigure
                    {
                        StartPoint = new Point(ToDrawMargin( Chart.Min.X, AxisTags.X),ToDrawMargin (Chart.Min.Y, AxisTags.Y)),
                        Segments = new PathSegmentCollection(
                            sp.Select(x => new LineSegment {Point = new Point(x.X, x.Y)}))
                    }
                })
            };

            var spath = new Path
            {
                StrokeThickness = 0,
                Data = sg,
                Fill = Fill,
                ClipToBounds = true
            };

            Chart.DrawMargin.Children.Add(path);
            addedFigures.Add(path);

            Chart.DrawMargin.Children.Add(spath);
            addedFigures.Add(spath);

            var draw = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromSeconds(0),
                KeyFrames = new DoubleKeyFrameCollection
                {
                    new SplineDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromMilliseconds(1),
                        Value = l
                    },
                    new SplineDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromMilliseconds(4000),
                        Value = 0
                    }
                }
            };
            Storyboard.SetTarget(draw, path);
            Storyboard.SetTargetProperty(draw, new PropertyPath(Shape.StrokeDashOffsetProperty));
            var sbDraw = new Storyboard();
            sbDraw.Children.Add(draw);
            var animated = false;
            if (!Chart.DisableAnimation)
            {
                if (animate)
                {
                    sbDraw.Begin();
                    animated = true;
                }
            }
            if (!animated) path.StrokeDashOffset = 0;
            return addedFigures;
        }

        private void AddLine(Point p1, Point p2, VisualHelper v1, VisualHelper v2, bool animate)
        {
            var l = new Line
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                X1 = p1.X,
                Y1 = p1.Y,
                X2 = p2.X,
                Y2 = p2.Y,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round
            };
            Shapes.Add(l);
            Chart.DrawMargin.Children.Add(l);
        }

        private void AddHoverAndClickShape(double minRadius, Point plotPoint, ChartPoint point, Shape e)
        {
            var r = new Rectangle
            {
                Fill = Brushes.Transparent,
                Width = minRadius*2,
                Height = minRadius*2,
                StrokeThickness = 0
            };

            r.MouseEnter += Chart.DataMouseEnter;
            r.MouseLeave += Chart.DataMouseLeave;
            r.MouseDown += Chart.DataMouseDown;

            Canvas.SetLeft(r, plotPoint.X - r.Width / 2);
            Canvas.SetTop(r, plotPoint.Y - r.Height / 2);
            Panel.SetZIndex(r, int.MaxValue);

            Chart.DrawMargin.Children.Add(r);

            Shapes.Add(r);
            Chart.ShapesMapper.Add(new ShapeMap { Series = this, HoverShape = r, ChartPoint = point, Shape = e });
        }

        private Shape AddPointMarkup(Point plotPoint)
        {
            var e = new Ellipse
            {
                Width = PointRadius * 2,
                Height = PointRadius * 2,
                Fill = Stroke,
                Stroke = new SolidColorBrush { Color = Chart.PointHoverColor },
                StrokeThickness = 1,
                ClipToBounds = true
            };

            Canvas.SetLeft(e, plotPoint.X - e.Width * .5);
            Canvas.SetTop(e, plotPoint.Y - e.Height * .5);

            Chart.DrawMargin.Children.Add(e);

            Shapes.Add(e);

            return e;
        }

        private void AddDataLabel(Point plotPoint, ChartPoint point, Func<double, string> f)
        {
            var tb = BuildATextBlock(0);
            var t = f(Chart.Invert ? point.X : point.Y);
            var ft = new FormattedText(t, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
            tb.Text = t;
            var l = plotPoint.X - ft.Width * .5;
            l = l < 0
                ? 0
                : (l + ft.Width > Chart.DrawMargin.Width
                    ? Chart.DrawMargin.Width - ft.Width
                    : l);
            var tp = plotPoint.Y - ft.Height - 5;
            tp = tp < 0 ? 0 : tp;
            Canvas.SetLeft(tb, l);
            Canvas.SetTop(tb, tp);
            Chart.DrawMargin.Children.Add(tb);
            Shapes.Add(tb);
        }

        internal override void Erase(bool force = false)
        {
            if (_isPrimitive)    //track by index
            {
                var activeIndexes = force || Values == null
                    ? new int[] { }
                    : Values.Points.Select(x => x.Key).ToArray();

                var inactiveIndexes = Chart.ShapesMapper
                    .Where(m => Equals(m.Series, this) &&
                                !activeIndexes.Contains(m.ChartPoint.Key))
                    .ToArray();
                foreach (var s in inactiveIndexes)
                {
                    var p = s.Shape.Parent as Canvas;
                    if (p != null)
                    {
                        p.Children.Remove(s.HoverShape);
                        p.Children.Remove(s.Shape);
                        Chart.ShapesMapper.Remove(s);
                        Shapes.Remove(s.Shape);
                    }
                }
            }
            else                //track by instance reference
            {
                var activeInstances = force ? new object[] { } : Values.Points.Select(x => x.Instance).ToArray();
                var inactiveIntances = Chart.ShapesMapper
                    .Where(m => Equals(m.Series, this) &&
                                !activeInstances.Contains(m.ChartPoint.Instance))
                    .ToArray();

                foreach (var s in inactiveIntances)
                {
                    var p = s.Shape.Parent as Canvas;
                    if (p != null)
                    {
                        p.Children.Remove(s.HoverShape);
                        p.Children.Remove(s.Shape);
                        Chart.ShapesMapper.Remove(s);
                        Shapes.Remove(s.Shape);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the next line of an instance or index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private LineVisualHelper GetLine(int index, object instance)
        {
            if (_isPrimitive)
            {
                if (_primitiveDictionary.ContainsKey(index))
                    return new LineVisualHelper
                    {
                        Line = _primitiveDictionary[index],
                        IsNew = false
                    };

                var l = new Line
                {
                    Stroke = Stroke,
                    StrokeThickness = StrokeThickness,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap = PenLineCap.Round
                };

                Shapes.Add(l);
                Chart.DrawMargin.Children.Add(l);

                _primitiveDictionary[index] = l;
                return new LineVisualHelper
                {
                    Line = l,
                    IsNew = true
                };
            }

            if (_dictionary.ContainsKey(instance))
                return new LineVisualHelper
                {
                    Line = _dictionary[instance],
                    IsNew = false
                };

            var li = new Line
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round
            };
            Shapes.Add(li);
            Chart.DrawMargin.Children.Add(li);

            _dictionary[instance] = li;
            return new LineVisualHelper
            {
                Line = li,
                IsNew = true
            };
        }

        private VisualHelper GetVisual(ChartPoint point)
        {
            var map = _isPrimitive
                ? Chart.ShapesMapper.FirstOrDefault(x => x.Series.Equals(this) &&
                                                         x.ChartPoint.Key == point.Key)
                : Chart.ShapesMapper.FirstOrDefault(x => x.Series.Equals(this) &&
                                                         x.ChartPoint.Instance == point.Instance);

            return map == null
                ? new VisualHelper
                {
                    PointShape = new Rectangle
                    {
                        StrokeThickness = StrokeThickness,
                        Stroke = Stroke,
                        Fill = Fill,
                        RenderTransform = new TranslateTransform()
                    },
                    HoverShape = new Rectangle
                    {
                        Fill = Brushes.Transparent,
                        StrokeThickness = 0
                    },
                    IsNew = true
                }
                : new VisualHelper
                {
                    PointShape = map.Shape,
                    HoverShape = map.HoverShape,
                    IsNew = false
                };
        }

        private struct LineVisualHelper
        {
            public bool IsNew { get; set; }
            public Line Line { get; set; }
        }

        private struct VisualHelper
        {
            public bool IsNew { get; set; }
            public Shape PointShape { get; set; }
            public Shape HoverShape { get; set; }
            public PointLines Lines { get; set; }
        }

        private struct PointLines
        {
            public Shape Left { get; set; }
            public Shape Right { get; set; }
        }
    }
}