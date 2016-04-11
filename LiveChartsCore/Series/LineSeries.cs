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
    public class LineSeries : Series
    {
        public static DateTime TestTimer = DateTime.Now;

        internal static readonly TimeSpan AnimSpeed = TimeSpan.FromMilliseconds(500);
        private bool _isPrimitive;
        private readonly LineSeriesTracker _tracker = new LineSeriesTracker();
        private readonly List<LineAndAreaShape> _areas = new List<LineAndAreaShape>();
        private readonly Canvas _extraElementsCanvas = new Canvas();

        public LineSeries()
        {
            StrokeThickness = 2.5;
            PointRadius = 4;
        }

        private ILine LineChart
        {
            get { return Chart as ILine; }
        }

        private bool RequeriesDataPoint
        {
            get { return Math.Abs(PointRadius) > .1; }
        }

        public double? LineSmoothness { get; set; }

        public double PointRadius { get; set; }

        public override void Plot(bool animate = true)
        {
            _isPrimitive = Values.Count >= 1 && Values[0].GetType().IsPrimitive;

            if (_extraElementsCanvas.Parent == null)
                Chart.DrawMargin.Children.Add(_extraElementsCanvas);

            _extraElementsCanvas.Width = Chart.DrawMargin.Width;
            _extraElementsCanvas.Height = Chart.DrawMargin.Height;

            var hoverShapeMinSize = PointRadius < 5 ? 5 : PointRadius;
            var f = (Chart.Invert ? CurrentXAxis : CurrentYAxis).GetFormatter();

            var s = 0;
            var so = 0;

            var isUp = Chart is IUnitaryPoints;

            foreach (var segment in Values.Points.AsSegments().Where(segment => segment.Count != 0))
            {
                LineAndAreaShape area;
                Point ofPt;

                var isNew = GetArea(s, isUp, out area, out ofPt);

                var p0 = ToDrawMargin(segment[0], ScalesXAt, ScalesYAt).AsPoint();
                area.Figure.StartPoint = isNew
                    ? (Chart.Invert
                        ? new Point(ToPlotArea(CurrentXAxis.MinLimit, AxisTags.X, ScalesXAt), p0.X)
                        : new Point(p0.X, ToPlotArea(CurrentYAxis.MinLimit, AxisTags.Y, ScalesYAt)))
                    : p0;
                area.Figure.BeginAnimation(PathFigure.StartPointProperty,
                    new PointAnimation(area.Figure.StartPoint,
                        segment.Count > 0 ? p0 : new Point(), AnimSpeed));

                Clue previous = null;
                var isVirgin = true;
                var first = new Point();
                var last = new Point();

                for (var i = 0; i < segment.Count; i++)
                {
                    var point = segment[i];
                    point.ChartLocation = ToDrawMargin(point, ScalesXAt, ScalesYAt).AsPoint();

                    if (isUp)
                        point.ChartLocation = new Point(point.ChartLocation.X + ofPt.X,
                            point.ChartLocation.Y + ofPt.Y);

                    if (isVirgin)
                    {
                        isVirgin = false;
                        first = point.ChartLocation;
                    }

                    var visual = GetVisual(segment[i], area.Figure);

                    PlaceVisual(visual, point.ChartLocation, hoverShapeMinSize);

                    if (DataLabels) Label(point, f, point.ChartLocation);

                    if (visual.IsNew) AddToCanvas(visual, point);

                    //var helper = GetSegmentHelper(point.Key, segment[i].Instance, area.Figure);
                    visual.Data = i == segment.Count - 1
                        ? new BezierData(previous != null ? previous.Data.Point3 : area.Figure.StartPoint)
                        //last line is a dummy line, just to keep algorithm simple.
                        : CalculateBezier(i, segment);
                    visual.Previous = previous != null && previous.IsNew ? previous.Previous : previous;
                    visual.Animate(i + so, Chart, so);
                    previous = visual;
                    last = point.ChartLocation;
                }

                if (area != null)
                    area.DrawLimits(first, last,
                        new Point(ToDrawMargin(CurrentXAxis.MinLimit, AxisTags.X, ScalesXAt),
                            ToDrawMargin(CurrentYAxis.MinLimit, AxisTags.Y, ScalesYAt)),
                        Chart.Invert);

#if DEBUG
                Trace.WriteLine("Segments count: " + area.Figure.Segments.Count);
#endif

                s++;
                so += segment.Count;
            }

            //instead of fire 10000 opacity animations for each point
            //we created a new control to hold all the point, then we wil just animate this Canvas
            _extraElementsCanvas.BeginAnimation(OpacityProperty,
                new DoubleAnimation(0, 1, Chart.DisableAnimation ? TimeSpan.MinValue : AnimSpeed));
        }

        private bool GetArea(int s, bool isUp, out LineAndAreaShape area, out Point ofPt)
        {
            var isNew = false;
            ofPt = new Point();

            if (_areas.Count <= s)
            {
                var path = new Path();
                BindingOperations.SetBinding(path, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath("Stroke"), Source = this });
                BindingOperations.SetBinding(path, Shape.FillProperty,
                    new Binding { Path = new PropertyPath("Fill"), Source = this });
                BindingOperations.SetBinding(path, Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath("StrokeThickness"), Source = this });
                BindingOperations.SetBinding(path, VisibilityProperty,
                    new Binding { Path = new PropertyPath("Visibility"), Source = this });
                BindingOperations.SetBinding(path, Panel.ZIndexProperty,
                    new Binding { Path = new PropertyPath(Panel.ZIndexProperty), Source = this });
                BindingOperations.SetBinding(path, Shape.StrokeDashArrayProperty,
                    new Binding { Path = new PropertyPath(StrokeDashArrayProperty), Source = this });
                var geometry = new PathGeometry();
                area = new LineAndAreaShape(new PathFigure(), path);
                geometry.Figures.Add(area.Figure);
                path.Data = geometry;
                _areas.Add(area);
                Chart.DrawMargin.Children.Add(path);
                isNew = true;
                if (isUp)
                {
                    if (Chart.Invert)
                    {
                        ofPt = new Point(0, Methods.GetUnitWidth(AxisTags.Y, Chart, ScalesYAt) * .5);
                        Canvas.SetTop(path, ofPt.Y);
                    }
                    else
                    {
                        ofPt = new Point(Methods.GetUnitWidth(AxisTags.X, Chart, ScalesXAt) * .5, 0);
                        Canvas.SetLeft(path, ofPt.X);
                    }
                }
            }
            else
            {
                area = _areas[s];
                if (isUp)
                {
                    if (Chart.Invert)
                    {
                        ofPt = new Point(0, Methods.GetUnitWidth(AxisTags.Y, Chart, ScalesYAt) * .5);
                        Canvas.SetTop(area.Path, ofPt.Y);
                    }
                    else
                    {
                        ofPt = new Point(Methods.GetUnitWidth(AxisTags.X, Chart, ScalesXAt) * .5, 0);
                        Canvas.SetLeft(area.Path, ofPt.X);
                    }
                }
            }
            return isNew;
        }

        private void PlaceVisual(Clue clue, Point pointLocation, double radius)
        {
            if (clue.HoverShape != null)
            {
                clue.HoverShape.Width = radius * 2;
                clue.HoverShape.Height = radius * 2;
                Canvas.SetLeft(clue.HoverShape, pointLocation.X - clue.HoverShape.Width * .5);
                Canvas.SetTop(clue.HoverShape, pointLocation.Y - clue.HoverShape.Height*.5);
            }

            if (clue.Ellipse != null)
            {
                Canvas.SetLeft(clue.Ellipse, pointLocation.X - clue.Ellipse.Width*.5);
                Canvas.SetTop(clue.Ellipse, pointLocation.Y - clue.Ellipse.Height*.5);
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

        private void AddToCanvas(Clue clue, ChartPoint point)
        {
            //Chart.ShapesMapper.Add(new ShapeMap
            //{
            //    Series = this,
            //    HoverShape = visual.HoverShape,
            //    Shape = visual.PointShape,
            //    ChartPoint = point
            //});

            //Removing the ChartPointReference migh be braking somehting
            //I think it is necesary for the tooltip
            //ToDo What is going here??

            if (clue.Ellipse != null)
            {
                _extraElementsCanvas.Children.Add(clue.Ellipse);
                Panel.SetZIndex(clue.Ellipse, int.MaxValue - 2);
            }

            if (clue.HoverShape != null)
            {
                Chart.DrawMargin.Children.Add(clue.HoverShape);
                Panel.SetZIndex(clue.HoverShape, int.MaxValue);
                clue.HoverShape.MouseDown += Chart.DataMouseDown;
                clue.HoverShape.MouseEnter += Chart.DataMouseEnter;
                clue.HoverShape.MouseLeave += Chart.DataMouseLeave;
            }
        }

        private BezierData CalculateBezier(int index, IList<ChartPoint> source)
        {
            var p1 = ToDrawMargin(source[index], ScalesXAt, ScalesYAt);
            var p2 = ToDrawMargin(source[index + 1], ScalesXAt, ScalesYAt);
            var p0 = index == 0 ? p1 : ToDrawMargin(source[index - 1], ScalesXAt, ScalesYAt);
            var p3 = index == source.Count - 2 ? p2 : ToDrawMargin(source[index + 2], ScalesXAt, ScalesYAt);

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
                Point1 = index == 0 ? new Point(p1.X, p1.Y) : new Point(c1X, c1Y),
                Point2 = index == source.Count ? new Point(p2.X, p2.Y) : new Point(c2X, c2Y),
                Point3 = new Point(p2.X, p2.Y)
            };
        }

        internal override void Erase(bool force = false)
        {
            if (Values == null) return;

            //track by index
            if (_isPrimitive)
                EreasePrimitives(force);

            //track by instance reference
            if (!_isPrimitive)
                EreaseInstances(force);

            foreach (var emptyArea in _areas
                .Where(a => !a.Figure.Segments.OfType<BezierSegment>().Any())
                .ToList())
            {
                Chart.DrawMargin.Children.Remove(emptyArea.Path);
                _areas.Remove(emptyArea);
            }

#if DEBUG
            if (_isPrimitive)
                Trace.WriteLine("Primitive dictionary count: " + _tracker.Primitives.Count);
            else
                Trace.WriteLine("Instance dictionary count: " + _tracker.Instances.Count);
#endif
        }

        private void EreasePrimitives(bool force)
        {
            var active = force || Values == null
                ? new int[] {}.AsEnumerable()
                : Values.Points.Select(x => x.Key);

            foreach (var key in _tracker.Primitives.Keys.Except(active))
            {
                var value = _tracker.Primitives[key];
                //var p = key.ChartPoint.Parent as Canvas; //Why This?
                //if (p != null)
                //{
                Chart.DrawMargin.Children.Remove(value.HoverShape);
                Chart.DrawMargin.Children.Remove(value.Ellipse);
                value.Owner.Segments.Remove(value.Segment);
                _tracker.Primitives.Remove(key);
                //}
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
                    var i = s.ChartPoint.Instance;
                    var bezier = _tracker.Instances[s.ChartPoint.Instance];
                    bezier.Owner.Segments.Remove(bezier.Segment);
                    bezier.Owner.Segments.Remove(bezier.Segment);
                    _tracker.Instances.Remove(i);
                }
            }
        }

        private Clue GetVisual(ChartPoint point, PathFigure pathFigure)
        {
            Clue trackable = null;

            if (_isPrimitive)
            {
                if (_tracker.Primitives.TryGetValue(point.Key, out trackable))
                {
                    trackable.IsNew = false;
                    return trackable;
                }
            }
            else
            {
                if (_tracker.Instances.TryGetValue(point.Instance, out trackable))
                {
                    trackable.IsNew = false;
                    return trackable;
                }
            }

            Ellipse e = null;
            Rectangle hs = null;

            if (RequeriesDataPoint)
            {
                e = new Ellipse
                {
                    Width = PointRadius*2,
                    Height = PointRadius*2,
                    Stroke = new SolidColorBrush {Color = Chart.PointHoverColor},
                    StrokeThickness = 1
                };
                BindingOperations.SetBinding(e, Shape.FillProperty,
                    new Binding {Path = new PropertyPath(StrokeProperty), Source = this});
                BindingOperations.SetBinding(e, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
            }

            if (Chart.Hoverable)
            {
                hs = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0
                };
                BindingOperations.SetBinding(hs, VisibilityProperty,
                    new Binding {Path = new PropertyPath(VisibilityProperty), Source = this});
            }

            trackable = new Clue(pathFigure);
            _tracker.Primitives[point.Key] = trackable;

            trackable.IsNew = true;
            trackable.HoverShape = hs;
            trackable.Ellipse = e;
            trackable.Segment = new BezierSegment();

            return trackable;

            //Todo: If Hoverable property changes thid could throw a null exception,
            //When Hoverable property changes, we need to load the shapes again.
            //or delete then if false, to improve performance.

            //map.ChartPoint.X = point.X; ToDo This might break something!!!
            //map.ChartPoint.Y = point.Y;
            //return new VisualHelper
            //{
            //    PointShape = map.Shape,
            //    HoverShape = map.HoverShape,
            //    IsNew = false
            //};
        }

        private struct VisualHelper
        {
            public bool IsNew { get; set; }
            public Shape PointShape { get; set; }
            public Shape HoverShape { get; set; }
        }
    }
}