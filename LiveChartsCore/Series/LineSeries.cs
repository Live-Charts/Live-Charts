//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez

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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Cache;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class LineSeries : Series
    {
        public static DateTime TestTimer = DateTime.Now;

        private static TimeSpan _animSpeed;
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

        private bool RequeriesDataPoint
        {
            get { return Math.Abs(PointRadius) > .1; }
        }

        public double? LineSmoothness { get; set; }

        public double PointRadius { get; set; }

        public override void Plot(bool animate = true)
        {
            _isPrimitive = Values.Count >= 1 && Values[0].GetType().IsPrimitive;

            _animSpeed = AnimationsSpeed ?? Chart.AnimationsSpeed;

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

                if (!Chart.DisableAnimations)
                    area.Figure.BeginAnimation(PathFigure.StartPointProperty,
                        new PointAnimation(area.Figure.StartPoint,
                            segment.Count > 0 ? p0 : new Point(), _animSpeed));

                DataPoint previous = null;
                var isVirgin = true;
                var first = new Point();
                var last = new Point();

                for (var i = 0; i < segment.Count; i++)
                {
                    var point = segment[i];
                    point.Location = ToDrawMargin(point, ScalesXAt, ScalesYAt).AsPoint();

                    if (isUp)
                        point.Location = new Point(point.Location.X + ofPt.X,
                            point.Location.Y + ofPt.Y);

                    if (isVirgin)
                    {
                        isVirgin = false;
                        first = point.Location;
                    }

                    var visual = GetVisual(segment[i], area.Figure);
                    PlaceVisuals(visual, point, hoverShapeMinSize, f);

                    visual.Data = i == segment.Count - 1
                        ? new BezierData(previous != null ? previous.Data.Point3 : area.Figure.StartPoint)
                        : CalculateBezier(i, segment);

                    visual.Previous = previous != null && previous.IsNew ? previous.Previous : previous;
                    visual.Animate(i + so, Chart, so, _animSpeed);

                    previous = visual;
                    last = point.Location;
                }

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

        private void PlaceVisuals(DataPoint dataPoint, ChartPoint chartPoint, double radius, Func<double, string> f)
        {
            if (dataPoint.HoverShape != null)
            {
                dataPoint.HoverShape.Width = radius * 2;
                dataPoint.HoverShape.Height = radius * 2;
                Canvas.SetLeft(dataPoint.HoverShape, chartPoint.Location.X - dataPoint.HoverShape.Width * .5);
                Canvas.SetTop(dataPoint.HoverShape, chartPoint.Location.Y - dataPoint.HoverShape.Height*.5);

                if (dataPoint.IsNew)
                {
                    Chart.DrawMargin.Children.Add(dataPoint.HoverShape);
                    Panel.SetZIndex(dataPoint.HoverShape, int.MaxValue);
                    dataPoint.HoverShape.MouseDown += Chart.DataMouseDown;
                    dataPoint.HoverShape.MouseEnter += Chart.DataMouseEnter;
                    dataPoint.HoverShape.MouseLeave += Chart.DataMouseLeave;
                }
            }

            if (dataPoint.Ellipse != null)
            {
                if (dataPoint.IsNew)
                {
                    Panel.SetZIndex(dataPoint.Ellipse, int.MaxValue - 2);
                    if (Chart.Invert)
                    {
                        Canvas.SetLeft(dataPoint.Ellipse, 0d);
                        Canvas.SetTop(dataPoint.Ellipse, chartPoint.Location.Y - dataPoint.Ellipse.Height*.5);
                    }
                    else
                    {
                        Canvas.SetLeft(dataPoint.Ellipse, chartPoint.Location.X - dataPoint.Ellipse.Width*.5);
                        Canvas.SetTop(dataPoint.Ellipse, Chart.DrawMargin.Height);
                    }
                    Chart.DrawMargin.Children.Add(dataPoint.Ellipse);
                }
                if (Chart.DisableAnimations)
                {
                    Canvas.SetTop(dataPoint.Ellipse, chartPoint.Location.Y - dataPoint.Ellipse.Height*.5);
                    Canvas.SetLeft(dataPoint.Ellipse, chartPoint.Location.X - dataPoint.Ellipse.Width * .5);
                }
                else
                {
                    dataPoint.Ellipse.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(chartPoint.Location.X - dataPoint.Ellipse.Width*.5, _animSpeed));
                    dataPoint.Ellipse.BeginAnimation(Canvas.TopProperty,
                        new DoubleAnimation(chartPoint.Location.Y - dataPoint.Ellipse.Height*.5, _animSpeed));
                }
            }

            if (dataPoint.TextBlock != null)
            {
                var te = f(Chart.Invert ? chartPoint.X : chartPoint.Y);

                dataPoint.TextBlock.Text = te;
                dataPoint.TextBlock.UpdateLayout();
                dataPoint.TextBlock.Measure(new Size(double.MaxValue, double.MaxValue));
                var ft = dataPoint.TextBlock.DesiredSize;

                var length = chartPoint.Location.X - ft.Width * .5;
                length = length < 0
                    ? 0
                    : (length + ft.Width > Chart.DrawMargin.Width
                        ? Chart.DrawMargin.Width - ft.Width
                        : length);
                var tp = chartPoint.Location.Y - ft.Height - 5;
                tp = tp < 0 ? 0 : tp;

                if (dataPoint.IsNew)
                {
                    Chart.DrawMargin.Children.Add(dataPoint.TextBlock);
                    Panel.SetZIndex(dataPoint.TextBlock, int.MaxValue - 2);

                    if (Chart.Invert)
                    {
                        Canvas.SetLeft(dataPoint.TextBlock, 0d);
                        Canvas.SetTop(dataPoint.TextBlock, tp);
                    }
                    else
                    {
                        Canvas.SetLeft(dataPoint.TextBlock, length);
                        Canvas.SetTop(dataPoint.TextBlock, Chart.DrawMargin.Height);
                    }
                }

                if (Chart.DisableAnimations)
                {
                    Canvas.SetTop(dataPoint.TextBlock, tp);
                    Canvas.SetLeft(dataPoint.TextBlock, length);
                } else
                {
                    dataPoint.TextBlock.BeginAnimation(Canvas.TopProperty, 
                        new DoubleAnimation(tp, _animSpeed));
                    dataPoint.TextBlock.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(length, _animSpeed));
                }
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

            foreach (var key in _tracker.Primitives.Keys.Except(active).ToArray())
            {
                var value = _tracker.Primitives[key];
                //var p = key.ChartPoint.Parent as Canvas; //Why This?
                //if (p != null)
                //{
                Chart.DrawMargin.Children.Remove(value.HoverShape);
                Chart.DrawMargin.Children.Remove(value.Ellipse);
                Chart.DrawMargin.Children.Remove(value.TextBlock);
                value.Owner.Segments.Remove(value.Segment);
                _tracker.Primitives.Remove(key);
                //}
            }
        }

        private void EreaseInstances(bool force)
        {
            var active = force || Values == null
                ? new object[] {}.AsEnumerable()
                : Values.Points.Select(x => x.Instance);

            foreach (var key in _tracker.Instances.Keys.Except(active).ToArray())
            {
                var value = _tracker.Instances[key];
                //var p = key.ChartPoint.Parent as Canvas; //Why This?
                //if (p != null)
                //{
                Chart.DrawMargin.Children.Remove(value.HoverShape);
                Chart.DrawMargin.Children.Remove(value.Ellipse);
                Chart.DrawMargin.Children.Remove(value.TextBlock);
                value.Owner.Segments.Remove(value.Segment);
                _tracker.Instances.Remove(key);
                //}
            }
        }

        private DataPoint GetVisual(ChartPoint point, PathFigure pathFigure)
        {
            DataPoint trackable;

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
            TextBlock tb = null;

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

            if (DataLabels)
                tb = BindATextBlock(0);

            trackable = new DataPoint(pathFigure)
            {
                IsNew = true,
                HoverShape = hs,
                Ellipse = e,
                TextBlock = tb,
                Segment = new BezierSegment()
            };

            if (_isPrimitive)
            {
                _tracker.Primitives[point.Key] = trackable;
            }
            else
            {
                _tracker.Instances[point.Instance] = trackable;
            }

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
    }
}