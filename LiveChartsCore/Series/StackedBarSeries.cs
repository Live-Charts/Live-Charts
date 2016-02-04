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
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class StackedBarSeries : Series
    {
        public StackedBarSeries()
        {
            StrokeThickness = 2.5;
            SetValue(ForegroundProperty, Brushes.WhiteSmoke);
        }

        public double StrokeThickness { get; set; }

        public override void Plot(bool animate = true)
        {
            if (Visibility != Visibility.Visible) return;
            if (Chart.Invert) PlotRow(animate);
            else PlotColumn(animate);
        }

        private void PlotRow(bool animate)
        {
            var stackedChart = Chart as IStackedBar;
            if (stackedChart == null) return;

            var stackedSeries = Chart.Series.OfType<StackedBarSeries>().ToList();

            var serieIndex = stackedSeries.IndexOf(this);
            var unitW = ToPlotArea(Chart.Max.Y - 1, AxisTags.Y) - Chart.PlotArea.Y + 5;
            var overflow = unitW - stackedChart.MaxColumnWidth > 0 ? unitW - stackedChart.MaxColumnWidth : 0;
            unitW = unitW > stackedChart.MaxColumnWidth ? stackedChart.MaxColumnWidth : unitW;
            var pointPadding = .1 * unitW;
            const int seriesPadding = 2;
            var h = unitW - 2 * pointPadding;

            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            foreach (var point in Values.Points)
            {
                var t = new TranslateTransform();

                var helper = stackedChart.IndexTotals[(int) point.Y];
                var w = ToPlotArea(helper.Total, AxisTags.X) - ToPlotArea(Chart.Min.X, AxisTags.X);
                var rh = w * (point.X / helper.Total);
                if (double.IsNaN(rh)) return;
                var stackedW = w * (helper.Stacked.ContainsKey(serieIndex) ? (helper.Stacked[serieIndex].Stacked / helper.Total) : 0);

                var r = new Rectangle
                {
                    StrokeThickness = StrokeThickness,
                    Stroke = Stroke,
                    Fill = Fill,
                    Width = 0,
                    Height = Math.Max(0, h - seriesPadding),
                    RenderTransform = t
                };
                var hr = new Rectangle
                {
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    Width = rh,
                    Height = Math.Max(0, h - seriesPadding)
                };

                Canvas.SetTop(r, ToPlotArea(point.Y, AxisTags.Y) + pointPadding + overflow / 2);
                Canvas.SetTop(hr, ToPlotArea(point.Y, AxisTags.Y) + pointPadding + overflow / 2);
                Canvas.SetLeft(hr, ToPlotArea(Chart.Min.X, AxisTags.X) + stackedW);
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
                    From = ToPlotArea(Chart.Min.X, AxisTags.X),
                    To = ToPlotArea(Chart.Min.X, AxisTags.X) + stackedW,
                    Duration = TimeSpan.FromMilliseconds(500)
                };

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
                    Chart.Canvas.Children.Add(tb);
                    Chart.Shapes.Add(tb);
                    Canvas.SetLeft(tb, Canvas.GetLeft(hr) + hr.Width*.5 - ft.Width*.5);
                    Canvas.SetTop(tb, Canvas.GetTop(hr) + hr.Height*.5 - ft.Height*.5);
                }

                var animated = false;
                if (!Chart.DisableAnimation)
                {
                    if (animate)
                    {
                        r.BeginAnimation(WidthProperty, hAnim);
                        t.BeginAnimation(TranslateTransform.XProperty, rAnim);
                        animated = true;
                    }
                }

                if (!animated)
                {
                    r.Width = rh;
                    if (rAnim.To != null) t.X = (double)rAnim.To;
                }

                hr.MouseDown += Chart.DataMouseDown;
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

        private void PlotColumn(bool animate)
        {
            var stackedChart = Chart as IStackedBar;
            if (stackedChart == null) return;

            var stackedSeries = Chart.Series.OfType<StackedBarSeries>().ToList();

            var serieIndex = stackedSeries.IndexOf(this);
            var unitW = ToPlotArea(1, AxisTags.X) - Chart.PlotArea.X + 5;
            var overflow = unitW - stackedChart.MaxColumnWidth > 0 ? unitW - stackedChart.MaxColumnWidth : 0;
            unitW = unitW > stackedChart.MaxColumnWidth ? stackedChart.MaxColumnWidth : unitW;
            var pointPadding = .1 * unitW;
            const int seriesPadding = 2;
            var barW = unitW - 2 * pointPadding;

            var f = Chart.GetFormatter(Chart.Invert ? Chart.AxisX : Chart.AxisY);

            foreach (var point in Values.Points)
            {
                var t = new TranslateTransform();

                var helper = stackedChart.IndexTotals[(int)point.X];
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

                Canvas.SetLeft(r, ToPlotArea(point.X, AxisTags.X) + pointPadding + overflow/2);
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
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                var rAnim = new DoubleAnimation
                {
                    From = ToPlotArea(Chart.Min.Y, AxisTags.Y),
                    To = ToPlotArea(Chart.Min.Y, AxisTags.Y) - rh - stackedH,
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

                if (DataLabels)
                {
                    var tb = BuildATextBlock(-90);
                    var te = f(Chart.Invert ? point.X : point.Y);
                    var ft = new FormattedText(
                        te,
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
                    tb.Text = te;
                    Chart.Canvas.Children.Add(tb);
                    Chart.Shapes.Add(tb);
                    Canvas.SetLeft(tb, Canvas.GetLeft(hr) + hr.Width*.5 - ft.Height*.5);
                    Canvas.SetTop(tb, Canvas.GetTop(hr) + hr.Height*.5 + ft.Width*.5);
                }

                if (!animated)
                {
                    r.Height = rh;
                    if (rAnim.To != null) t.Y = (double)rAnim.To;
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
