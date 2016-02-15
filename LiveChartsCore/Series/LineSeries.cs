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
using System.Diagnostics;
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
        public static DateTime TestTimer = DateTime.Now;

        internal static readonly TimeSpan AnimSpeed = TimeSpan.FromMilliseconds(500);
        private bool _isPrimitive;
        private readonly LineSeriesDictionaries _dictionaries = new LineSeriesDictionaries();
        private readonly PathFigure _figure;
        private readonly Path _path;

        public LineSeries()
        {
            StrokeThickness = 2.5;
            PointRadius = 4;

            _path = new Path();
            var geometry = new PathGeometry();
            _figure = new PathFigure();
            geometry.Figures.Add(_figure);
            _path.Data = geometry;
        }

        private ILine LineChart
        {
            get { return Chart as ILine; }
        }

        public double? LineSmoothness { get; set; }
        public double StrokeThickness { get; set; }

        public double PointRadius { get; set; }

        public override void Plot(bool animate = true)
        {
            if (_path.Parent == null) Chart.DrawMargin.Children.Add(_path);

            _path.Stroke = Stroke;
            _path.StrokeThickness = StrokeThickness;
            //previous lines feels bad, this should not be pulled this ugly
            //both properties should be binded, and taking all wpf power.
            //also with visibiliy property.
            //ToDo: do this as it should!

            _isPrimitive = Values.Count >= 1 && Values[0].GetType().IsPrimitive;

            //This is so ugly now and will not work for 0.6.5 //Todo: Fix this.
            if (Visibility != Visibility.Visible) return;

            var rr = PointRadius < 5 ? 5 : PointRadius;
            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

#if DEBUG
            TestTimer = DateTime.Now;
            Trace.WriteLine("Line Series Plotting, animation scheduled to last " + AnimSpeed.TotalMilliseconds + "ms");
#endif

            foreach (var segment in Values.Points.AsSegments())
            {
                var p0 = ToDrawMargin(segment[0]).AsPoint();
                _figure.StartPoint = p0;
                _figure.BeginAnimation(PathFigure.StartPointProperty, new PointAnimation(_figure.StartPoint,
                    segment.Count > 0 ? p0 : new Point(), AnimSpeed));

                PathSegmentHelper previous = null;
                PathSegmentHelper next = null;

                for (var i = 0; i < segment.Count - 1; i++)
                {
                    var point = segment[i];
                    var pointLocation = ToDrawMargin(point).AsPoint();

                    var visual = GetVisual(segment[i]);
                    visual.HoverShape.Width = rr * 2;
                    visual.HoverShape.Height = rr * 2;
                    Canvas.SetLeft(visual.PointShape, pointLocation.X - visual.PointShape.Width * .5);
                    Canvas.SetTop(visual.PointShape, pointLocation.Y - visual.PointShape.Height * .5);
                    Canvas.SetLeft(visual.HoverShape, pointLocation.X - visual.HoverShape.Width * .5);
                    Canvas.SetTop(visual.HoverShape, pointLocation.Y - visual.HoverShape.Height * .5);

                    visual.PointShape.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 0, TimeSpan.FromMilliseconds(1)));
                    if (!Chart.DisableAnimation)
                    {
                        var pt = new DispatcherTimer {Interval = AnimSpeed};
                        pt.Tick += (sender, args) =>
                        {
                            //instead we can ad everything to a labels panel, and only animaty opacity of the panel
                            //this runs alot of unecesary animations.
                            //ToDo: ^
                            visual.PointShape.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, AnimSpeed));
                            pt.Stop();
                        };
                        pt.Start();
                    }
                    else
                    {
                        visual.PointShape.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(1)));
                    }

                    if (DataLabels)
                    {
                        //this could be cleaner too
                        //Another ToDo
                        var tb = BuildATextBlock(0);
                        var te = f(Chart.Invert ? point.X : point.Y);
                        var ft = new FormattedText(te, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                            new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
                        tb.Text = te;
                        var length = pointLocation.X - ft.Width*.5;
                        length = length < 0
                            ? 0
                            : (length + ft.Width > Chart.DrawMargin.Width
                                ? Chart.DrawMargin.Width - ft.Width
                                : length);
                        var tp = pointLocation.Y - ft.Height - 5;
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
                            var t = new DispatcherTimer {Interval = AnimSpeed};
                            t.Tick += (sender, args) =>
                            {
                                tb.Visibility = Visibility.Visible;
                                var fadeIn = new DoubleAnimation(0, 1, AnimSpeed);
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

                    var helper = GetSegmentHelper(i, segment[i].Instance);
                    helper.Data = CalculateBezier(i, segment);
                    helper.Previous = previous != null && previous.IsNew ? previous.Previous : previous;
                    helper.Animate(i, _figure, Chart);
                    previous = helper;
                }
            }
        }

        private BezierData CalculateBezier(int index, IList<ChartPoint> source)
        {
            var p1 = ToDrawMargin(source[index]);
            var p2 = ToDrawMargin(source[index + 1]);
            var p0 = index == 0 ? p1 : ToDrawMargin(source[index - 1]);
            var p3 = index == source.Count - 2 ? p2 : ToDrawMargin(source[index + 2]);

            var xc1 = (p0.X + p1.X) / 2.0;
            var yc1 = (p0.Y + p1.Y) / 2.0;
            var xc2 = (p1.X + p2.X) / 2.0;
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

            var smoothness = LineSmoothness ?? LineChart.LineSmoothness;
            smoothness = smoothness > 1 ? 1 : (smoothness < 0 ? 0 : smoothness);

            var c1X = xm1 + (xc2 - xm1) * smoothness + p1.X - xm1;
            var c1Y = ym1 + (yc2 - ym1) * smoothness + p1.Y - ym1;
            var c2X = xm2 + (xc2 - xm2) * smoothness + p2.X - xm2;
            var c2Y = ym2 + (yc2 - ym2) * smoothness + p2.Y - ym2;

            return new BezierData
            {
                P1 = index == 0 ? new Point(p1.X, p1.Y) : new Point(c1X, c1Y),
                P2 = index == source.Count ? new Point(p2.X, p2.Y) : new Point(c2X, c2Y),
                P3 = new Point(p2.X, p2.Y)
            };
        }

        internal override void Erase(bool force = false)
        {
            //track by index
            if (_isPrimitive)
                EreasePrimitives(force);

            //track by instance reference
            if (!_isPrimitive)
                EreaseInstances(force);
        }

        private void EreasePrimitives(bool force)
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
                    BezierSegment bezier;
                    if (_dictionaries.Primitives.TryGetValue(i, out bezier))
                    {
                        //WARNING this could cause memory leaks
                        //because we do only remove bezier, but we never remove Path from canvas
                        //ToDo: delete this memory leak, maybe asking at the end of this method if: !figure.Segments.Any() then remove _path from canvas
                        _figure.Segments.Remove(bezier);
                        _dictionaries.Primitives.Remove(i);
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

        private void EreaseInstances(bool force)
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
                    BezierSegment bezier;
                    if (_dictionaries.Instances.TryGetValue(i, out bezier))
                    {
                        //WARNING this could cause memory leaks
                        //because we do only remove bezier, but we never remove Path from canvas
                        //ToDo: delete this memory leak, maybe asking at the end of this method if: !figure.Segments.Any() then remove _path from canvas
                        _figure.Segments.Remove(bezier);
                        _dictionaries.Instances.Remove(i);
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
        private PathSegmentHelper GetSegmentHelper(int index, object instance)
        {
            if (_isPrimitive)
            {
                if (_dictionaries.Primitives.ContainsKey(index))
                    return new PathSegmentHelper
                    {
                        Segment = _dictionaries.Primitives[index],
                        IsNew = false
                    };

                var primitiveBezier = new BezierSegment();
                _dictionaries.Primitives[index] = primitiveBezier;

                return new PathSegmentHelper
                {
                    Segment = primitiveBezier,
                    IsNew = true
                };
            }

            if (_dictionaries.Instances.ContainsKey(instance))
                return new PathSegmentHelper
                {
                    Segment = _dictionaries.Instances[instance],
                    IsNew = false
                };

            var instanceBezier = new BezierSegment();
            _dictionaries.Instances[instance] = instanceBezier;

            return new PathSegmentHelper
            {
                Segment = instanceBezier,
                IsNew = true
            };
        }

        private VisualHelper GetVisual(ChartPoint point)
        {
            //could we implement a dictionary here? 
            //when we have large amounts of sata this should run slowly
            //ToDo: check if ShapesMapper could be a dictionary so we can access values faster.
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
                Primitives = new Dictionary<int, BezierSegment>();
                Instances = new Dictionary<object, BezierSegment>();
            }
            public Dictionary<int, BezierSegment> Primitives { get; set; }
            public Dictionary<object, BezierSegment> Instances { get; set; }
        }
    }

    internal struct BezierData
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Point P3 { get; set; }
        public Point StartPoint { get; set; }

        public BezierSegment AssignTo(BezierSegment segment)
        {
            segment.Point1 = P1;
            segment.Point2 = P2;
            segment.Point3 = P3;
            return segment;
        }
    }

    internal class PathSegmentHelper
    {
        public bool IsNew { get; set; }
        public BezierSegment Segment { get; set; }
        public BezierData Data { get; set; }
        public PathSegmentHelper Previous { get; set; }
        public PathSegmentHelper Next { get; set; }

        public void Animate(int index, PathFigure figure, Chart chart)
        {
            var s1 = new Point();
            var s2 = new Point();
            var s3 = new Point();

            if (IsNew)
            {
                figure.Segments.Insert(index, Data.AssignTo(Segment));
                if (chart.Invert)
                {
                    s1 = new Point(chart.ToDrawMargin(chart.Min.X, AxisTags.X), Data.P1.Y);
                    s2 = new Point(chart.ToDrawMargin(chart.Min.X, AxisTags.X), Data.P2.Y);
                    s3 = new Point(chart.ToDrawMargin(chart.Min.X, AxisTags.X), Data.P3.Y);
                }
                else
                {
                    s1 = new Point(Data.P1.X, chart.ToDrawMargin(chart.Min.Y, AxisTags.Y));
                    s2 = new Point(Data.P2.X, chart.ToDrawMargin(chart.Min.Y, AxisTags.Y));
                    s3 = new Point(Data.P3.X, chart.ToDrawMargin(chart.Min.Y, AxisTags.Y));
                }
            }

            var p1 = IsNew ? (Previous != null && !Previous.IsNew ? Previous.Segment.Point3 : s1) : Segment.Point1;
            var p2 = IsNew ? (Previous != null && !Previous.IsNew ? Previous.Segment.Point3 : s2) : Segment.Point2;
            var p3 = IsNew ? (Previous != null && !Previous.IsNew ? Previous.Segment.Point3 : s3) : Segment.Point3;

            if (chart.DisableAnimation)
            {
                Segment.Point1 = Data.P1;
                Segment.Point2 = Data.P2;
                Segment.Point3 = Data.P3;
                return;
            }

            Segment.BeginAnimation(BezierSegment.Point1Property,
                new PointAnimation(p1, Data.P1, LineSeries.AnimSpeed));
            Segment.BeginAnimation(BezierSegment.Point2Property,
                new PointAnimation(p2, Data.P2, LineSeries.AnimSpeed));
            Segment.BeginAnimation(BezierSegment.Point3Property,
                new PointAnimation(p3, Data.P3, LineSeries.AnimSpeed));
        }
    }
}