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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace lvc
{
    public class StackedBarSeries : Series
    {
        public StackedBarSeries()
        {
            StrokeThickness = 2.5;
        }

        public double StrokeThickness { get; set; }

        public override void Plot(bool animate = true)
        {
            var chart = Chart as IStackedBar;
            if (chart == null) return;

            var stackedSeries = Chart.Series.OfType<StackedBarSeries>().ToList();

            var serieIndex = stackedSeries.IndexOf(this);
            var unitW = ToPlotArea(1, AxisTags.X) - Chart.PlotArea.X + 5;
            var overflow = unitW - chart.MaxColumnWidth > 0 ? unitW - chart.MaxColumnWidth : 0;
            unitW = unitW > chart.MaxColumnWidth ? chart.MaxColumnWidth : unitW;
            var pointPadding = .1 * unitW;
            const int seriesPadding = 2;
            var barW = unitW - 2 * pointPadding;

            foreach (var point in Values.Points)
            {
                var t = new TranslateTransform();

                var helper = chart.IndexTotals[(int) point.X];
                var barH = ToPlotArea(Chart.Min.Y, AxisTags.Y) - ToPlotArea(helper.Total, AxisTags.Y);
                var rh = barH * (point.Y / helper.Total);
                if (double.IsNaN(rh)) return;
                var stackedH = barH * (helper.Stacked.ContainsKey(serieIndex) ? (helper.Stacked[serieIndex].Stacked / helper.Total) : 0);

                var r = new Rectangle
                {
                    StrokeThickness = StrokeThickness,
                    Stroke = Stroke,
                    Fill = Fill,
                    Width = Math.Max(0, barW - seriesPadding),
                    Height = 0,
                    RenderTransform = t
                };
                var hr = new Rectangle
                {
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    Width = Math.Max(0, barW - seriesPadding),
                    Height = rh
                };

                Canvas.SetLeft(r, ToPlotArea(point.X, AxisTags.X) + pointPadding + overflow / 2);
                Canvas.SetLeft(hr, ToPlotArea(point.X, AxisTags.X) + pointPadding + overflow / 2);
                Canvas.SetTop(hr, ToPlotArea(Chart.Min.Y, AxisTags.Y) - rh - stackedH);
                Panel.SetZIndex(hr, int.MaxValue);

                Chart.Canvas.Children.Add(r);
                Chart.Canvas.Children.Add(hr);
                Shapes.Add(r);
                Shapes.Add(hr);

                var hAnim = new DoubleAnimation
                {
                    To = rh,
                    Duration = TimeSpan.FromMilliseconds(300)
                };
                var rAnim = new DoubleAnimation
                {
                    From = ToPlotArea(Chart.Min.Y, AxisTags.Y),
                    To = ToPlotArea(Chart.Min.Y, AxisTags.Y) - rh - stackedH,
                    Duration = TimeSpan.FromMilliseconds(300)
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
                    if (rAnim.To != null) t.Y = (double)rAnim.To;
                }

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
