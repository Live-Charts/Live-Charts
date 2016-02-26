//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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

namespace LiveCharts
{
    public class BarSeries : Series
    {
        private int animationSpeed = 500;
        private bool _isPrimitive;

        public BarSeries()
        {
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (double), typeof (BarSeries), new PropertyMetadata(2.5));

        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public override void Plot(bool animate = true)
        {
            _isPrimitive = Values == null || (Values.Count >= 1 && Values[0].GetType().IsPrimitive);

            if (Chart.Invert)
                PlotRows();
            else
                PlotColumns();
        }

        private void PlotRows()
        {
            var chart = Chart as IBar;
            if (chart == null) return;
            var barSeries = Chart.Series.OfType<BarSeries>().ToList();
            var pos = barSeries.IndexOf(this);
            var count = barSeries.Count;
            var unitW = ToPlotArea(Chart.Max.Y - 1, AxisTags.Y) - Chart.PlotArea.Y + 5;
            var overflow = unitW - chart.MaxColumnWidth * 3 > 0 ? unitW - chart.MaxColumnWidth * 3 : 0;
            unitW = unitW > chart.MaxColumnWidth * 3 ? chart.MaxColumnWidth * 3 : unitW;

            var pointPadding = .1 * unitW;
            const int seriesPadding = 2;
            var barW = (unitW - 2 * pointPadding) / count;

            var bothLimitsPositive = Chart.Max.X > 0 && Chart.Min.X > 0 - Chart.S.X * .01;
            var bothLimitsNegative = Chart.Max.X < 0 + Chart.S.X * .01 && Chart.Min.X < 0;

            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            foreach (var point in Values.Points)
            {
                var visual = GetVisual(point);
                var height = Math.Max(0, barW - seriesPadding);

                var barStart = bothLimitsPositive
                    ? Chart.Min.X
                    : (bothLimitsNegative ? Chart.Max.X : 0);

                var direction = point.X > 0 ? 1 : -1;

                var rw = bothLimitsNegative
                    ? ToPlotArea(point.X, AxisTags.X)
                    : ToPlotArea(point.X*direction, AxisTags.X) - ToPlotArea(barStart, AxisTags.X);

                var w = direction > 0 ? ToPlotArea(barStart, AxisTags.X) : ToPlotArea(barStart, AxisTags.X) - rw;

                var top = ToPlotArea(point.Y, AxisTags.Y) + barW*pos + pointPadding + overflow/2;

                Canvas.SetTop(visual.HoverShape, top);
                Canvas.SetLeft(visual.HoverShape, w);
                visual.HoverShape.Width = rw;
                visual.HoverShape.Height = height;
                visual.PointShape.Height = height;

                Canvas.SetTop(visual.PointShape, top);

                if (!Chart.DisableAnimation)
                {
                    var wAnim = new DoubleAnimation
                    {
                        From = visual.IsNew
                            ? 0
                            : visual.PointShape.Width,
                        To = rw,
                        Duration = TimeSpan.FromMilliseconds(animationSpeed)
                    };
                    var leftAnim = new DoubleAnimation
                    {
                        From = visual.IsNew
                            ? ToPlotArea(barStart, AxisTags.X)
                            : Canvas.GetLeft(visual.PointShape),
                        To = w,
                        Duration = TimeSpan.FromMilliseconds(animationSpeed)
                    };
                    visual.PointShape.BeginAnimation(WidthProperty, wAnim);
                    visual.PointShape.BeginAnimation(Canvas.LeftProperty, leftAnim);
                }
                else
                {
                    Canvas.SetLeft(visual.PointShape, w);
                    visual.PointShape.Width = rw;
                }

                if (DataLabels)
                {
                    var tb = BuildATextBlock(0);
                    var te = f(Chart.Invert ? point.X : point.Y);
                    var ft = new FormattedText(
                        te,
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
                    tb.Text = te;
                    tb.Visibility = Visibility.Hidden;
                    Chart.Canvas.Children.Add(tb);
                    Chart.Shapes.Add(tb);
                    Canvas.SetLeft(tb,
                        direction > 0
                            ? Canvas.GetLeft(visual.HoverShape) + visual.HoverShape.Width + 5
                            : Canvas.GetLeft(visual.HoverShape) - 5 - ft.Width);
                    Canvas.SetTop(tb, Canvas.GetTop(visual.HoverShape) + visual.HoverShape.Height*.5 - ft.Height*.5);
                    Panel.SetZIndex(tb, int.MaxValue - 1);
                    if (!Chart.DisableAnimation)
                    {
                        var t = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(animationSpeed) };
                        t.Tick += (sender, args) =>
                        {
                            tb.Visibility = Visibility.Visible;
                            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(animationSpeed));
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
                    Chart.Canvas.Children.Add(visual.PointShape);
                    Chart.Canvas.Children.Add(visual.HoverShape);
                    Shapes.Add(visual.PointShape);
                    Shapes.Add(visual.HoverShape);
                    Panel.SetZIndex(visual.HoverShape, int.MaxValue);
                    Panel.SetZIndex(visual.PointShape, int.MaxValue - 2);
                    visual.HoverShape.MouseDown += Chart.DataMouseDown;
                    visual.HoverShape.MouseEnter += Chart.DataMouseEnter;
                    visual.HoverShape.MouseLeave += Chart.DataMouseLeave;
                }
            }
        }

        private void PlotColumns()
        {
            var chart = Chart as IBar;
            if (chart == null) return;
            var barSeries = Chart.Series.OfType<BarSeries>().ToList();
            var pos = barSeries.IndexOf(this);
            var count = barSeries.Count;
            var unitW = ToPlotArea(1, AxisTags.X) - Chart.PlotArea.X + 5;
            var overflow = unitW - chart.MaxColumnWidth * 3 > 0 ? unitW - chart.MaxColumnWidth * 3 : 0;
            unitW = unitW > chart.MaxColumnWidth * 3 ? chart.MaxColumnWidth * 3 : unitW;

            var pointPadding = .1 * unitW;
            const int seriesPadding = 2;
            var barW = (unitW - 2 * pointPadding) / count;

            var bothLimitsPositive = Chart.Max.Y > 0 && Chart.Min.Y > 0 - Chart.S.Y * .01;
            var bothLimitsNegative = Chart.Max.Y < 0 + Chart.S.Y * .01 && Chart.Min.Y < 0;

            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            foreach (var point in Values.Points)
            {
                var visual = GetVisual(point);
                var width = Math.Max(0, barW - seriesPadding);

                var barStart = bothLimitsPositive
                    ? Chart.Min.Y
                    : (bothLimitsNegative ? Chart.Max.Y : 0);

                var direction = point.Y > 0 ? 1 : -1;

                var rh = bothLimitsNegative
                    ? ToPlotArea(point.Y, AxisTags.Y)
                    : ToPlotArea(barStart, AxisTags.Y) - ToPlotArea(point.Y * direction, AxisTags.Y);
                var h = direction > 0 ? ToPlotArea(barStart, AxisTags.Y) - rh : ToPlotArea(barStart, AxisTags.Y);

                var left = ToPlotArea(point.X, AxisTags.X) + barW*pos + pointPadding + overflow/2;

                Canvas.SetLeft(visual.HoverShape, left);
                Canvas.SetTop(visual.HoverShape, h);
                visual.HoverShape.Height = rh;
                visual.HoverShape.Width = width;
                visual.PointShape.Width = width;

                Canvas.SetLeft(visual.PointShape, left);

                if (!Chart.DisableAnimation)
                {
                    var hAnim = new DoubleAnimation
                    {
                        From = visual.IsNew
                            ? 0
                            : visual.PointShape.Height,
                        To = rh,
                        Duration = TimeSpan.FromMilliseconds(animationSpeed)
                    };
                    var topAnim = new DoubleAnimation
                    {
                        From = visual.IsNew
                            ? ToPlotArea(barStart, AxisTags.Y)
                            : Canvas.GetTop(visual.PointShape),
                        To = h,
                        Duration = TimeSpan.FromMilliseconds(animationSpeed)
                    };

                    visual.PointShape.BeginAnimation(HeightProperty, hAnim);
                    visual.PointShape.BeginAnimation(Canvas.TopProperty, topAnim);
                }
                else
                {
                    Canvas.SetTop(visual.PointShape, h);
                    visual.PointShape.Height = rh;
                }

                if (DataLabels)
                {
                    var tb = BuildATextBlock(0);
                    var te = f(Chart.Invert ? point.X : point.Y);
                    var ft = new FormattedText(
                        te,
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
                    tb.Text = te;
                    tb.Visibility = Visibility.Hidden;
                    Chart.Canvas.Children.Add(tb);
                    Chart.Shapes.Add(tb);
                    Canvas.SetLeft(tb, Canvas.GetLeft(visual.HoverShape) + visual.HoverShape.Width * .5 - ft.Width * .5);
                    Canvas.SetTop(tb,
                        direction > 0
                            ? Canvas.GetTop(visual.HoverShape) - ft.Height - 5
                            : Canvas.GetTop(visual.HoverShape) + visual.HoverShape.Height + 5);
                    Panel.SetZIndex(tb, int.MaxValue - 1);
                    if (!Chart.DisableAnimation)
                    {
                        var t = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(animationSpeed) };
                        t.Tick += (sender, args) =>
                        {
                            tb.Visibility = Visibility.Visible;
                            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(animationSpeed));
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
                    Chart.Canvas.Children.Add(visual.PointShape);
                    Chart.Canvas.Children.Add(visual.HoverShape);
                    Shapes.Add(visual.PointShape);
                    Shapes.Add(visual.HoverShape);
                    Panel.SetZIndex(visual.HoverShape, int.MaxValue);
                    Panel.SetZIndex(visual.PointShape, int.MaxValue - 2);
                    visual.HoverShape.MouseDown += Chart.DataMouseDown;
                    visual.HoverShape.MouseEnter += Chart.DataMouseEnter;
                    visual.HoverShape.MouseLeave += Chart.DataMouseLeave;
                }
            }
        }

        internal override void Erase(bool force = false)
        {
            if (_isPrimitive)    //track by index
            {
                var activeIndexes = force || Values == null
                    ? new int[] {}
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
                var activeInstances = force ? new object[] {} : Values.Points.Select(x => x.Instance).ToArray();
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

        private VisualHelper GetVisual(ChartPoint point)
        {
            var map = _isPrimitive
                ? Chart.ShapesMapper.FirstOrDefault(x => x.Series.Equals(this) &&
                                                         x.ChartPoint.Key == point.Key)
                : Chart.ShapesMapper.FirstOrDefault(x => x.Series.Equals(this) &&
                                                         x.ChartPoint.Instance == point.Instance);

            if (map == null)
            {
                var r = new Rectangle
                {
                    RenderTransform = new TranslateTransform()
                };
                var hs = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0
                };
                BindingOperations.SetBinding(r, Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath("Stroke"), Source = this });
                BindingOperations.SetBinding(r, Shape.FillProperty,
                    new Binding { Path = new PropertyPath("Fill"), Source = this });
                BindingOperations.SetBinding(r, Shape.StrokeThicknessProperty,
                    new Binding { Path = new PropertyPath("StrokeThickness"), Source = this });
                BindingOperations.SetBinding(r, VisibilityProperty,
                    new Binding { Path = new PropertyPath("Visibility"), Source = this });
                BindingOperations.SetBinding(hs, VisibilityProperty,
                    new Binding { Path = new PropertyPath("Visibility"), Source = this });

                return new VisualHelper
                {
                    PointShape = r,
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
