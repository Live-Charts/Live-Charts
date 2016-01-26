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
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class BarSeries : Series
    {
        public BarSeries()
        {
            StrokeThickness = 2.5;
        }

        public double StrokeThickness { get; set; }

        public override void Plot(bool animate = true)
        {
            if (Chart.Invert)
                PlotRows(animate);
            else
                PlotColumns(animate);
        }

        private void PlotRows(bool animate)
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

            foreach (var point in Values.Points)
            {
                var t = new TranslateTransform();
                var r = new Rectangle
                {
                    StrokeThickness = StrokeThickness,
                    Stroke = Stroke,
                    Fill = Fill,
                    Width = Math.Max(0, barW - seriesPadding),
                    Height = 0,
                    RenderTransform = t
                };

                var barStart = bothLimitsPositive
                    ? Chart.Min.Y
                    : (bothLimitsNegative ? Chart.Max.Y : 0);
                var direction = point.Y > 0 ? 1 : -1;

                var rh = bothLimitsNegative
                    ? ToPlotArea(point.Y, AxisTags.Y)
                    : ToPlotArea(barStart, AxisTags.Y) - ToPlotArea(point.Y * direction, AxisTags.Y);
                var hr = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0,
                    Width = Math.Max(0, barW - seriesPadding),
                    Height = rh
                };

                Canvas.SetLeft(r, ToPlotArea(point.X, AxisTags.X) + barW * pos + pointPadding + overflow / 2);
                Canvas.SetLeft(hr, ToPlotArea(point.X, AxisTags.X) + barW * pos + pointPadding + overflow / 2);

                var h = direction > 0 ? ToPlotArea(barStart, AxisTags.Y) - rh : ToPlotArea(barStart, AxisTags.Y);

                Canvas.SetTop(hr, h);
                Panel.SetZIndex(hr, int.MaxValue);

                Chart.Canvas.Children.Add(r);
                Chart.Canvas.Children.Add(hr);
                Shapes.Add(r);
                Shapes.Add(hr);

                var hAnim = new DoubleAnimation
                {
                    To = rh,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                var rAnim = new DoubleAnimation
                {
                    From = ToPlotArea(barStart, AxisTags.Y),
                    To = h,
                    Duration = TimeSpan.FromMilliseconds(500)
                };

                var animated = false;
                if (!Chart.DisableAnimation)
                {
                    if (animate)
                    {
                        r.BeginAnimation(HeightProperty, hAnim);
                        t.BeginAnimation(TranslateTransform.YProperty, rAnim);
                        animated = true;
                    }
                }

                if (!animated)
                {
                    r.Height = rh;
                    t.Y = h;
                }

                hr.MouseDown += Chart.DataMouseDown;

                if (!Chart.Hoverable) continue;
                hr.MouseEnter += Chart.DataMouseEnter;
                hr.MouseLeave += Chart.DataMouseLeave;

                Chart.HoverableShapes.Add(new HoverableShape
                {
                    Series = this,
                    Shape = hr,
                    Target = r,
                    Value = point
                });
            }
        }

        private void PlotColumns(bool animate)
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

            foreach (var point in Values.Points)
            {
                var t = new TranslateTransform();
                var r = new Rectangle
                {
                    StrokeThickness = StrokeThickness,
                    Stroke = Stroke,
                    Fill = Fill,
                    Width = Math.Max(0, barW - seriesPadding),
                    Height = 0,
                    RenderTransform = t
                };

                var barStart = bothLimitsPositive
                    ? Chart.Min.Y
                    : (bothLimitsNegative ? Chart.Max.Y : 0);
                var direction = point.Y > 0 ? 1 : -1;

                var rh = bothLimitsNegative
                    ? ToPlotArea(point.Y, AxisTags.Y)
                    : ToPlotArea(barStart, AxisTags.Y) - ToPlotArea(point.Y * direction, AxisTags.Y);
                var hr = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0,
                    Width = Math.Max(0, barW - seriesPadding),
                    Height = rh
                };

                Canvas.SetLeft(r, ToPlotArea(point.X, AxisTags.X) + barW * pos + pointPadding + overflow / 2);
                Canvas.SetLeft(hr, ToPlotArea(point.X, AxisTags.X) + barW * pos + pointPadding + overflow / 2);

                var h = direction > 0 ? ToPlotArea(barStart, AxisTags.Y) - rh : ToPlotArea(barStart, AxisTags.Y);

                Canvas.SetTop(hr, h);
                Panel.SetZIndex(hr, int.MaxValue);

                Chart.Canvas.Children.Add(r);
                Chart.Canvas.Children.Add(hr);
                Shapes.Add(r);
                Shapes.Add(hr);

                var hAnim = new DoubleAnimation
                {
                    To = rh,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                var rAnim = new DoubleAnimation
                {
                    From = ToPlotArea(barStart, AxisTags.Y),
                    To = h,
                    Duration = TimeSpan.FromMilliseconds(500)
                };

                var animated = false;
                if (!Chart.DisableAnimation)
                {
                    if (animate)
                    {
                        r.BeginAnimation(HeightProperty, hAnim);
                        t.BeginAnimation(TranslateTransform.YProperty, rAnim);
                        animated = true;
                    }
                }

                if (!animated)
                {
                    r.Height = rh;
                    t.Y = h;
                }

                hr.MouseDown += Chart.DataMouseDown;

                if (!Chart.Hoverable) continue;
                hr.MouseEnter += Chart.DataMouseEnter;
                hr.MouseLeave += Chart.DataMouseLeave;

                Chart.HoverableShapes.Add(new HoverableShape
                {
                    Series = this,
                    Shape = hr,
                    Target = r,
                    Value = point
                });
            }
        }
    }
}
