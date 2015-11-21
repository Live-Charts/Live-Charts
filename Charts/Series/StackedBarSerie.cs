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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;

namespace LiveCharts.Series
{
    public class StackedBarSerie : Serie
    {
        public override void Plot(bool animate = true)
        {
            var chart = Chart as StackedBarChart;
            if (chart == null) return;

            var serieIndex = Chart.Series.IndexOf(this);
            var unitW = ToPlotArea(1, AxisTags.X) - Chart.PlotArea.X + 5;
            var overflow = unitW - chart.MaxColumnWidth > 0 ? unitW - chart.MaxColumnWidth : 0;
            unitW = unitW > chart.MaxColumnWidth ? chart.MaxColumnWidth : unitW;
            var pointPadding = .1 * unitW;
            const int seriesPadding = 2;
            var barW = unitW - 2 * pointPadding;

            for (var index = 0; index < PrimaryValues.Count; index++)
            {
                var d = PrimaryValues[index];

                var t = new TranslateTransform();
                var r = new Rectangle
                {
                    StrokeThickness = StrokeThickness,
                    Stroke = new SolidColorBrush {Color = Color},
                    Fill = new SolidColorBrush {Color = Color, Opacity = .8},
                    Width = barW - seriesPadding,
                    Height = 0,
                    RenderTransform = t
                };

                var helper = chart.IndexTotals[index];
                var barH = ToPlotArea(Chart.Min.Y, AxisTags.Y) - ToPlotArea(helper.Total, AxisTags.Y);
                var rh = barH*(d/helper.Total);
                var stackedH = barH*(helper.Stacked[serieIndex].Stacked/helper.Total);

                Canvas.SetLeft(r, ToPlotArea(index, AxisTags.X) + pointPadding + overflow/2);

                Chart.Canvas.Children.Add(r);
                Shapes.Add(r);

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
                        r.BeginAnimation(FrameworkElement.HeightProperty, hAnim);
                        t.BeginAnimation(TranslateTransform.YProperty, rAnim);
                        animated = true;
                    }
                }

                if (!animated)
                {
                    r.Height = rh;
                    t.Y = (double) rAnim.To;
                }

                if (!Chart.Hoverable) continue;
                r.MouseEnter += Chart.DataMouseEnter;
                r.MouseLeave += Chart.DataMouseLeave;
                Chart.HoverableShapes.Add(new HoverableShape
                {
                    Serie = this,
                    Shape = r,
                    Target = r,
                    Value = new Point(index, d)
                });
            }
        }
    }
}
