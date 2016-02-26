//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez, algorithm based on Raul Otaño Hurtado article.

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
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts.CoreComponents;
using LiveCharts.Helpers;

namespace LiveCharts
{
    public class LineSeries : CoreComponents.Series
    {
        public static DateTime TestTimer = DateTime.Now;

        internal static readonly TimeSpan AnimSpeed = TimeSpan.FromMilliseconds(500);
        private bool _isPrimitive;
        private readonly LineSeriesTracker _tracker = new LineSeriesTracker();
        private readonly List<LineAndAreaShape> _areas = new List<LineAndAreaShape>();

        public LineSeries()
        {
            StrokeThickness = 2.5;
            PointRadius = 4;
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
            _isPrimitive = Values.Count >= 1 && Values[0].GetType().IsPrimitive;

            var rr = PointRadius < 5 ? 5 : PointRadius;
            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            var s = 0;
            var so = 0;
            foreach (var segment in Values.Points.AsSegments().Where(segment => segment.Count != 0))
            {
                LineAndAreaShape area;

                if (_areas.Count <= s)
                {
                    var path = new Path
                    {
                        Stroke = Stroke, StrokeThickness = StrokeThickness, Fill = Fill
                    };
                    BindingOperations.SetBinding(path, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath("Stroke"), Source = this });
                    BindingOperations.SetBinding(path, Shape.FillProperty,
                        new Binding { Path = new PropertyPath("Fill"), Source = this });
                    BindingOperations.SetBinding(path, Shape.StrokeThicknessProperty,
                        new Binding { Path = new PropertyPath("StrokeThickness"), Source = this });
                    BindingOperations.SetBinding(path, VisibilityProperty,
                        new Binding { Path = new PropertyPath("Visibility"), Source = this });
                    var geometry = new PathGeometry();
                    area = new LineAndAreaShape(new PathFigure());
                    geometry.Figures.Add(area.Figure);
                    path.Data = geometry;
                    _areas.Add(area);
                    Chart.DrawMargin.Children.Add(path);
                }
                else
                {
                    area = _areas[s];
                }

                var p0 = ToDrawMargin(segment[0]).AsPoint();
                area.Figure.StartPoint = p0;
                area.Figure.BeginAnimation(PathFigure.StartPointProperty, new PointAnimation(area.Figure.StartPoint,
                    segment.Count > 0 ? p0 : new Point(), AnimSpeed));

                AnimatableSegments previous = null;
                var isVirgin = true;
                var first = new Point();
                var last = new Point();

                for (var i = 0; i < segment.Count; i++)
                {
                    var point = segment[i];
                    var pointLocation = ToDrawMargin(point).AsPoint();

                    if (isVirgin)
                    {
                        isVirgin = false;
                        first = pointLocation;
                    }

                    var visual = GetVisual(segment[i]);

                    PlaceVisual(visual, pointLocation, rr);

                    if (DataLabels) Label(point, f, pointLocation);

                    if (visual.IsNew) AddToCanvas(visual, point);

                    var helper = GetSegmentHelper(point.Key, segment[i].Instance, area.Figure);
                    helper.Data = i == segment.Count - 1
                        ? new BezierData(previous != null ? previous.Data.P3 : area.Figure.StartPoint)
                        //last line is a dummy line, just to keep algorithm simple.
                        : CalculateBezier(i, segment);
                    helper.Previous = previous != null && previous.IsNew ? previous.Previous : previous;
                    helper.Animate(i + so, Chart, so);
                    previous = helper;
                    last = pointLocation;
                }

                if (area != null)
                    area.DrawLimits(first, last,
                        new Point(ToDrawMargin(Chart.Min.X, AxisTags.X), ToDrawMargin(Chart.Min.Y, AxisTags.Y)),
                        Chart.Invert);

#if DEBUG
                Trace.WriteLine("Segments count: " + area.Figure.Segments.Count);
#endif

                s++;
                so += segment.Count;
            }
        }

        private void PlaceVisual(VisualHelper visual, Point pointLocation, double radius)
        {
            visual.HoverShape.Width = radius * 2;
            visual.HoverShape.Height = radius * 2;
            Canvas.SetLeft(visual.PointShape, pointLocation.X - visual.PointShape.Width * .5);
            Canvas.SetTop(visual.PointShape, pointLocation.Y - visual.PointShape.Height * .5);
            Canvas.SetLeft(visual.HoverShape, pointLocation.X - visual.HoverShape.Width * .5);
            Canvas.SetTop(visual.HoverShape, pointLocation.Y - visual.HoverShape.Height * .5);

            visual.PointShape.BeginAnimation(OpacityProperty,
                new DoubleAnimation(0, 0, TimeSpan.FromMilliseconds(1)));

            if (!Chart.DisableAnimation)
            {
                var pt = new DispatcherTimer { Interval = AnimSpeed };
                pt.Tick += (sender, args) =>
                {
                    visual.PointShape.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, AnimSpeed));
                    pt.Stop();
                };
                pt.Start();
            }
            else
            {
                visual.PointShape.BeginAnimation(OpacityProperty,
                    new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(1)));
            }
        }

        private void Label(ChartPoint point, Func<double, string> f, Point pointLocation)
        {
            var tb = BuildATextBlock(0);
            var te = f(Chart.Invert ? point.X : point.Y);
            var ft = new FormattedText(te, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
            tb.Text = te;
            var length = pointLocation.X - ft.Width * .5;
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
                var t = new DispatcherTimer { Interval = AnimSpeed };
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

        private void AddToCanvas(VisualHelper visual, ChartPoint point)
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

#if DEBUG
            if (_isPrimitive)
                Trace.WriteLine("Primitive dictionary count: " + _tracker.Primitives.Count);
            else
                Trace.WriteLine("Instance dictionary count: " + _tracker.Instances.Count);
#endif
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
                    var bezier = _tracker.Primitives[s.ChartPoint.Key];
                    bezier.Owner.Segments.Remove(bezier.Segment);
                    _tracker.Primitives.Remove(i);
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
                    var bezier = _tracker.Instances[s.ChartPoint.Instance];
                    bezier.Owner.Segments.Remove(bezier.Segment);
                    bezier.Owner.Segments.Remove(bezier.Segment);
                    _tracker.Instances.Remove(i);
                }
            }
        }

        /// <summary>
        /// Gets the next line of an instance or index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="instance"></param>
        /// <param name="lineFigure"></param>
        /// <returns></returns>
        private AnimatableSegments GetSegmentHelper(int index, object instance, PathFigure lineFigure)
        {
            if (_isPrimitive)
            {
                if (_tracker.Primitives.ContainsKey(index))
                {
                    var primitive = _tracker.Primitives[index];
                    return new AnimatableSegments
                    {
                        Bezier = primitive,
                        IsNew = false
                    };
                }

                var primitiveBeziers = new TrackableBezier(lineFigure) {Segment = new BezierSegment()};

                _tracker.Primitives[index] = primitiveBeziers;

                return new AnimatableSegments
                {
                    Bezier = primitiveBeziers,
                    IsNew = true
                };
            }

            if (_tracker.Instances.ContainsKey(instance))
            {
                var instanceVal = _tracker.Instances[instance];
                return new AnimatableSegments
                {
                    Bezier = instanceVal,
                    IsNew = false
                };
            }

            var instanceBeziers = new TrackableBezier(lineFigure) {Segment = new BezierSegment()};
            _tracker.Instances[instance] = instanceBeziers;

            return new AnimatableSegments
            {
                Bezier = instanceBeziers,
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

            if (map == null)
            {
                var e = new Ellipse
                {
                    Width = PointRadius*2,
                    Height = PointRadius*2,
                    Stroke = new SolidColorBrush {Color = Chart.PointHoverColor},
                    StrokeThickness = 1
                };
                var hs = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0
                };
                BindingOperations.SetBinding(e, Shape.FillProperty,
                    new Binding { Path = new PropertyPath("Fill"), Source = this });
                BindingOperations.SetBinding(e, VisibilityProperty,
                    new Binding { Path = new PropertyPath("Visibility"), Source = this });
                BindingOperations.SetBinding(hs, VisibilityProperty,
                    new Binding { Path = new PropertyPath("Visibility"), Source = this });
                return new VisualHelper
                {
                    PointShape = e,
                    HoverShape = hs,
                    IsNew = true
                };
            }
            
            map.ChartPoint.X = point.X;
            map.ChartPoint.Y = point.Y;
            return new VisualHelper
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
    }
}