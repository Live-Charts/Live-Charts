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
            var chart = Chart as IBar;
            if (chart == null) return;
            var barSeries = Chart.Series.OfType<BarSeries>().ToList();
            var pos = barSeries.IndexOf(this);
            var count = barSeries.Count;
            var unitW = ToPlotArea(1, AxisTags.X) - Chart.PlotArea.X + 5;
            var overflow = unitW - chart.MaxColumnWidth*3 > 0 ? unitW - chart.MaxColumnWidth*3 : 0;
            unitW = unitW > chart.MaxColumnWidth*3 ? chart.MaxColumnWidth*3 : unitW;

            var pointPadding = .1*unitW;
            const int seriesPadding = 2;
            var barW = (unitW - 2*pointPadding)/count;

            foreach (var point in ChartPoints)
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
               
                var rh = ToPlotArea(Chart.Min.Y, AxisTags.Y) - ToPlotArea(point.Y, AxisTags.Y);

                Canvas.SetLeft(r, ToPlotArea(point.X, AxisTags.X) + barW*pos + pointPadding + overflow/2);

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
                    To = ToPlotArea(Chart.Min.Y, AxisTags.Y) - rh,
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
                    t.Y = ToPlotArea(Chart.Min.Y, AxisTags.Y) - rh;
                }

                if (!Chart.Hoverable) continue;
                r.MouseEnter += Chart.DataMouseEnter;
                r.MouseLeave += Chart.DataMouseLeave;
                Chart.HoverableShapes.Add(new HoverableShape
                {
                    Series = this,
                    Shape = r,
                    Target = r,
                    Value = point
                });
            }
        }
    }
}
