//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez and Raul Otaño Hurtado

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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class LineSeries : Series
    {
        private TimeSpan d = TimeSpan.FromMilliseconds(500);
        private bool _isPrimitive;
        private LineSeriesDictionaries _dictionaries = new LineSeriesDictionaries();

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
            
            if (LineChart.LineType == LineChartLineType.Polyline)
                PlotLines();

            PlotBeziers();
        }

        /// <summary>
        /// Algorithm brought by Raul Otaño Hurtado, adapted and modified by Beto Rodríguez
        /// </summary>
        private void PlotBeziers()
        {
            var rr = PointRadius < 5 ? 5 : PointRadius;
            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            foreach (var segment in Values.Points.AsSegments())
            {
                for (var i = 0; i < segment.Count-1; i++)
                {
                    var p1 = ToDrawMargin(segment[i]);
                    var p2 = ToDrawMargin(segment[i + 1]);
                    var p0 = i == 0 ? p1 : ToDrawMargin(segment[i - 1]);
                    var p3 = i == segment.Count - 2 ? p2 : ToDrawMargin(segment[i + 2]);

                    var xc1 = (p0.X + p1.X) / 2.0;
                    var yc1 = (p0.Y + p1.Y) / 2.0;
                    var xc2 = (p1.X + p2.Y) / 2.0;
                    var yc2 = (p1.Y + p2.Y) / 2.0;
                    var xc3 = (p2.X + p3.X) / 2.0;
                    var yc3 = (p2.Y + p3.Y) / 2.0;

                    var len1 = Math.Sqrt((p1.X - p0.X) * (p1.X - p0.X) + (p1.Y - p0.Y) * (p1.Y - p0.Y));
                    var len2 = Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
                    var len3 = Math.Sqrt((p3.X - p2.X) * (p3.X - p2.X) + (p3.Y - p2.Y) * (p3.Y - p2.Y));

                    var k1 = len1 / (len1 + len2);
                    var k2 = len2 / (len2 + len3);

                    var xm1 = xc1 + (xc2 - xc1) * k1;
                    var ym1 = yc1 + (yc2 - yc1) * k1;

                    var xm2 = xc2 + (xc3 - xc2) * k2;
                    var ym2 = yc2 + (yc3 - yc2) * k2;

                    const double smoothValue = 0.8;

                    var ctrl1X = xm1 + (xc2 - xm1) * smoothValue + p1.X - xm1;
                    var ctrl1Y = ym1 + (yc2 - ym1) * smoothValue + p1.Y - ym1;

                    var ctrl2X = xm2 + (xc2 - xm2) * smoothValue + p2.X - xm2;
                    var ctrl2Y = ym2 + (yc2 - ym2) * smoothValue + p2.Y - ym2;

                }
            }
        }

        /// <summary>
        /// Algorithm by Beto Rodríguez
        /// </summary>
        private void PlotLines()
        {
            var rr = PointRadius < 5 ? 5 : PointRadius;
            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            foreach (var segment in Values.Points.AsSegments())
            {
                var previous = PlotLineByIndex(0, segment, new LineVisualHelper {Line = null}, rr, f);
                var point = segment[0];
                var location = new Point(ToDrawMargin(point.X, AxisTags.X), ToDrawMargin(point.Y, AxisTags.Y));

                if (previous.IsNew)
                {
                    previous.Line.X1 = location.X;
                    previous.Line.Y1 = location.Y;
                }
                else
                {
                    previous.Line.BeginAnimation(Line.X1Property, new DoubleAnimation(previous.Line.X1, location.X, d));
                    previous.Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(previous.Line.Y1, location.Y, d));
                }

                for (int index = 1; index < segment.Count; index++)
                {
                    previous = PlotLineByIndex(index, segment, previous, rr, f);
                }
            }
        }

        private LineVisualHelper PlotBezierByIndex(int index, IList<ChartPoint> source, LineVisualHelper previous, double rr, Func<double, string> f)
        {
            var point = source[index];
            var visual = GetVisual(point);

            var currentLine = new LineVisualHelper();
            if (index != source.Count - 1) currentLine = GetLine(index, point.Instance);
            var location = new Point(ToDrawMargin(point.X, AxisTags.X), ToDrawMargin(point.Y, AxisTags.Y));

            visual.HoverShape.Width = rr * 2;
            visual.HoverShape.Height = rr * 2;
            Canvas.SetLeft(visual.PointShape, location.X - visual.PointShape.Width * .5);
            Canvas.SetTop(visual.PointShape, location.Y - visual.PointShape.Height * .5);
            Canvas.SetLeft(visual.HoverShape, location.X - visual.HoverShape.Width * .5);
            Canvas.SetTop(visual.HoverShape, location.Y - visual.HoverShape.Height * .5);

            visual.PointShape.Visibility = Visibility.Hidden;
            if (!Chart.DisableAnimation)
            {
                var pt = new DispatcherTimer { Interval = d };
                pt.Tick += (sender, args) =>
                {
                    visual.PointShape.Visibility = Visibility.Visible;
                    pt.Stop();
                };
                pt.Start();
            }
            else
            {
                visual.PointShape.Visibility = Visibility.Visible;
            }

            var x = previous.Line == null ? location.X : previous.Line.X2;
            var y = previous.Line == null ? location.Y : previous.Line.Y2;

            if (previous.Line != null)
            {
                if (index != source.Count - 1)
                {
                    var px = previous.IsNew ? location.X : previous.Line.X2;
                    var py = previous.IsNew ? location.Y : previous.Line.Y2;
                    currentLine.Line.BeginAnimation(Line.X1Property, new DoubleAnimation(px, location.X, d));
                    currentLine.Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(py, location.Y, d));
                }
                var cx = previous.IsNew ? location.X : x;
                var cy = previous.IsNew ? location.Y : y;
                previous.Line.BeginAnimation(Line.X2Property, new DoubleAnimation(cx, location.X, d));
                previous.Line.BeginAnimation(Line.Y2Property, new DoubleAnimation(cy, location.Y, d));
            }

            if (DataLabels)
            {
                var tb = BuildATextBlock(0);
                var te = f(Chart.Invert ? point.X : point.Y);
                var ft = new FormattedText(te, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
                tb.Text = te;
                var length = location.X - ft.Width * .5;
                length = length < 0
                    ? 0
                    : (length + ft.Width > Chart.DrawMargin.Width
                        ? Chart.DrawMargin.Width - ft.Width
                        : length);
                var tp = location.Y - ft.Height - 5;
                tp = tp < 0 ? 0 : tp;
                tb.Text = te;
                tb.Visibility = Visibility.Hidden;
                Chart.Canvas.Children.Add(tb);
                Chart.Shapes.Add(tb);
                Canvas.SetLeft(tb, length + Canvas.GetLeft(Chart.DrawMargin));
                Canvas.SetTop(tb, tp + Canvas.GetTop(Chart.DrawMargin));
                Panel.SetZIndex(tb, int.MaxValue - 1);
                if (!Chart.DisableAnimation)
                {
                    var t = new DispatcherTimer { Interval = d };
                    t.Tick += (sender, args) =>
                    {
                        tb.Visibility = Visibility.Visible;
                        var fadeIn = new DoubleAnimation(0, 1, d);
                        tb.BeginAnimation(OpacityProperty, fadeIn);
                        t.Stop();
                    };
                    t.Start();
                }
                else
                {
                    tb.Visibility = Visibility.Visible;
                }
            }
            if (visual.IsNew)
            {
                Chart.ShapesMapper.Add(new ShapeMap
                {
                    Series = this,
                    HoverShape = visual.HoverShape,
                    Shape = visual.PointShape,
                    ChartPoint = point
                });
                Chart.DrawMargin.Children.Add(visual.PointShape);
                Chart.DrawMargin.Children.Add(visual.HoverShape);
                Shapes.Add(visual.PointShape);
                Shapes.Add(visual.HoverShape);
                Panel.SetZIndex(visual.HoverShape, int.MaxValue);
                Panel.SetZIndex(visual.PointShape, int.MaxValue - 2);
                visual.HoverShape.MouseDown += Chart.DataMouseDown;
                visual.HoverShape.MouseEnter += Chart.DataMouseEnter;
                visual.HoverShape.MouseLeave += Chart.DataMouseLeave;
            }
            return currentLine;
        }

        private LineVisualHelper PlotLineByIndex(int index, IList<ChartPoint> source, LineVisualHelper previous, double rr, Func<double, string> f)
        {
            var point = source[index];
            var visual = GetVisual(point);

            var currentLine = new LineVisualHelper();
            if (index != source.Count - 1) currentLine = GetLine(index, point.Instance);
            var location = new Point(ToDrawMargin(point.X, AxisTags.X), ToDrawMargin(point.Y, AxisTags.Y));

            visual.HoverShape.Width = rr * 2;
            visual.HoverShape.Height = rr * 2;
            Canvas.SetLeft(visual.PointShape, location.X - visual.PointShape.Width * .5);
            Canvas.SetTop(visual.PointShape, location.Y - visual.PointShape.Height * .5);
            Canvas.SetLeft(visual.HoverShape, location.X - visual.HoverShape.Width * .5);
            Canvas.SetTop(visual.HoverShape, location.Y - visual.HoverShape.Height * .5);

            visual.PointShape.Visibility = Visibility.Hidden;
            if (!Chart.DisableAnimation)
            {
                var pt = new DispatcherTimer {Interval = d};
                pt.Tick += (sender, args) =>
                {
                    visual.PointShape.Visibility = Visibility.Visible;
                    pt.Stop();
                };
                pt.Start();
            }
            else
            {
                visual.PointShape.Visibility = Visibility.Visible;
            }

            var x = previous.Line == null ? location.X : previous.Line.X2;
            var y = previous.Line == null ? location.Y : previous.Line.Y2;

            if (previous.Line != null)
            {
                if (index != source.Count - 1)
                {
                    var px = previous.IsNew ? location.X : previous.Line.X2;
                    var py = previous.IsNew ? location.Y : previous.Line.Y2;
                    currentLine.Line.BeginAnimation(Line.X1Property, new DoubleAnimation(px, location.X, d));
                    currentLine.Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(py, location.Y, d));
                }
                var cx = previous.IsNew ? location.X : x;
                var cy = previous.IsNew ? location.Y : y;
                previous.Line.BeginAnimation(Line.X2Property, new DoubleAnimation(cx, location.X, d));
                previous.Line.BeginAnimation(Line.Y2Property, new DoubleAnimation(cy, location.Y, d));
            }

            if (DataLabels)
            {
                var tb = BuildATextBlock(0);
                var te = f(Chart.Invert ? point.X : point.Y);
                var ft = new FormattedText(te, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
                tb.Text = te;
                var length = location.X - ft.Width * .5;
                length = length < 0
                    ? 0
                    : (length + ft.Width > Chart.DrawMargin.Width
                        ? Chart.DrawMargin.Width - ft.Width
                        : length);
                var tp = location.Y - ft.Height - 5;
                tp = tp < 0 ? 0 : tp;
                tb.Text = te;
                tb.Visibility = Visibility.Hidden;
                Chart.Canvas.Children.Add(tb);
                Chart.Shapes.Add(tb);
                Canvas.SetLeft(tb, length + Canvas.GetLeft(Chart.DrawMargin));
                Canvas.SetTop(tb, tp + Canvas.GetTop(Chart.DrawMargin));
                Panel.SetZIndex(tb, int.MaxValue - 1);
                if (!Chart.DisableAnimation)
                {
                    var t = new DispatcherTimer { Interval = d };
                    t.Tick += (sender, args) =>
                    {
                        tb.Visibility = Visibility.Visible;
                        var fadeIn = new DoubleAnimation(0, 1, d);
                        tb.BeginAnimation(OpacityProperty, fadeIn);
                        t.Stop();
                    };
                    t.Start();
                }
                else
                {
                    tb.Visibility = Visibility.Visible;
                }
            }
            if (visual.IsNew)
            {
                Chart.ShapesMapper.Add(new ShapeMap
                {
                    Series = this,
                    HoverShape = visual.HoverShape,
                    Shape = visual.PointShape,
                    ChartPoint = point
                });
                Chart.DrawMargin.Children.Add(visual.PointShape);
                Chart.DrawMargin.Children.Add(visual.HoverShape);
                Shapes.Add(visual.PointShape);
                Shapes.Add(visual.HoverShape);
                Panel.SetZIndex(visual.HoverShape, int.MaxValue);
                Panel.SetZIndex(visual.PointShape, int.MaxValue - 2);
                visual.HoverShape.MouseDown += Chart.DataMouseDown;
                visual.HoverShape.MouseEnter += Chart.DataMouseEnter;
                visual.HoverShape.MouseLeave += Chart.DataMouseLeave;
            }
            return currentLine;
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

        internal override void Erase(bool force = false)
        {
            //track by index
            if (_isPrimitive && LineChart.LineType == LineChartLineType.Polyline)
                EreasePrimitiveLines(force);

            //track by instance reference
            if (!_isPrimitive && LineChart.LineType == LineChartLineType.Polyline)
                EreaseInstanceLines(force);
        }

        private void EreasePrimitiveLines(bool force)
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
                    var i = s.ChartPoint.Key;
                    Line l;
                    if (_dictionaries.PrimitiveLines.TryGetValue(i, out l))
                    {
                        Chart.DrawMargin.Children.Remove(l);
                        Shapes.Remove(l);
                        _dictionaries.PrimitiveLines.Remove(i);
                    }
                    else
                    {
#if DEBUG
                        //this means a shape was erased but it is not in the dictionary, how could this be possible?
                        //this normally throws when you call Chart.Update, it should never fail unless you call that method, why??
                        //ToDo: find this issue, Priority Low, since it is not common to happen, only occurs when you need to force redraw.
                        System.Diagnostics.Trace.TraceWarning(
                            "Could not find ChartPoint instance '{0}' in dictionary", i);
#endif
                    }
                }
            }
        }

        private void EreaseInstanceLines(bool force)
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
                    var i = s.ChartPoint.Instance;
                    Line l;
                    if (_dictionaries.Lines.TryGetValue(i, out l))
                    {
                        Chart.DrawMargin.Children.Remove(l);
                        Shapes.Remove(l);
                        _dictionaries.Lines.Remove(i);
                    }
                    else
                    {
#if DEBUG
                        //this means a shape was erased but it is not in the dictionary, how could this be possible?
                        //this normally throws when you call Chart.Update, it should never fail unless you call that method, why??
                        //ToDo: find this issue, Priority Low, since it is not common to happen, only occurs when you need to force redraw.
                        System.Diagnostics.Trace.TraceWarning(
                            "Could not find ChartPoint instance '{0}' in dictionary", i);
#endif
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
                if (_dictionaries.PrimitiveLines.ContainsKey(index))
                    return new LineVisualHelper
                    {
                        Line = _dictionaries.PrimitiveLines[index],
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

                _dictionaries.PrimitiveLines[index] = l;
                return new LineVisualHelper
                {
                    Line = l,
                    IsNew = true
                };
            }

            if (_dictionaries.Lines.ContainsKey(instance))
                return new LineVisualHelper
                {
                    Line = _dictionaries.Lines[instance],
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

            _dictionaries.Lines[instance] = li;
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
                    PointShape = new Ellipse
                    {
                        Width = PointRadius * 2,
                        Height = PointRadius * 2,
                        Fill = Stroke,
                        Stroke = new SolidColorBrush { Color = Chart.PointHoverColor },
                        StrokeThickness = 1
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

        private struct BezierVisualHelper
        {
            public bool IsNew { get; set; }
            public BezierSegment Bezier { get; set; }
        }

        private struct VisualHelper
        {
            public bool IsNew { get; set; }
            public Shape PointShape { get; set; }
            public Shape HoverShape { get; set; }
        }

        private class LineSeriesDictionaries
        {
            public LineSeriesDictionaries()
            {
                PrimitiveLines = new Dictionary<int, Line>();
                Lines = new Dictionary<object, Line>();
                PrimitiveBeziers = new Dictionary<int, BezierSegment>();
                Beziers = new Dictionary<object, BezierSegment>();
            }
            public Dictionary<int, Line> PrimitiveLines { get; set; }
            public Dictionary<object, Line> Lines { get; set; }
            public Dictionary<int, BezierSegment> PrimitiveBeziers { get; set; }
            public Dictionary<object, BezierSegment> Beziers { get; set; }
        }
    }
}