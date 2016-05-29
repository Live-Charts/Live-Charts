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

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Charts;

namespace LiveCharts.Wpf.Charts.Chart
{
    public abstract partial class Chart
    {

        public void SetDrawMarginTop(double value)
        {
            Canvas.SetTop(DrawMargin, value);
        }

        public void SetDrawMarginLeft(double value)
        {
            Canvas.SetLeft(DrawMargin, value);
        }

        public void SetDrawMarginHeight(double value)
        {
            DrawMargin.Height = value;
        }

        public void SetDrawMarginWidth(double value)
        {
            DrawMargin.Width = value;
        }

        public void AddToView(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            Canvas.Children.Add(wpfElement);
        }

        public void AddToDrawMargin(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            DrawMargin.Children.Add(wpfElement);
        }

        public void RemoveFromView(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            Canvas.Children.Remove(wpfElement);
        }

        public void RemoveFromDrawMargin(object element)
        {
            var wpfElement = element as FrameworkElement;
            if (wpfElement == null) return;
            DrawMargin.Children.Remove(wpfElement);
        }

        public void ShowLegend(CorePoint at)
        {
            if (ChartLegend == null) return;

            if (ChartLegend.Parent == null)
            {
                AddToView(ChartLegend);
                Canvas.SetLeft(ChartLegend, 0d);
                Canvas.SetTop(ChartLegend, 0d);
            }

            ChartLegend.Visibility = Visibility.Visible;

            Canvas.SetLeft(ChartLegend, at.X);
            Canvas.SetTop(ChartLegend, at.Y);
        }

        public void HideLegend()
        {
            if (ChartLegend != null)
                ChartLegend.Visibility = Visibility.Hidden;
        }

        public CoreSize LoadLegend()
        {
            if (ChartLegend == null || LegendLocation == LegendLocation.None)
                return new CoreSize();

            if (ChartLegend.Parent == null)
                Canvas.Children.Add(ChartLegend);

            var l = new List<SeriesViewModel>();

            for (var i = 0; i < Series.Count; i++)
            {
                var item = new SeriesViewModel();

                var series = (Series.Series) Series[i];

                item.Title = series.Title;
                item.StrokeThickness = series.StrokeThickness;
                item.Stroke = series.Stroke ?? new SolidColorBrush(GetDefaultColor(i));
                item.Fill = series.Fill ?? new SolidColorBrush(GetDefaultColor(i)) {Opacity = series.DefaultFillOpacity};

                l.Add(item);
            }

            ChartLegend.Series = l;

            ChartLegend.InternalOrientation = LegendLocation == LegendLocation.Bottom ||
                                              LegendLocation == LegendLocation.Top
                ? Orientation.Horizontal
                : Orientation.Vertical;

            ChartLegend.UpdateLayout();
            ChartLegend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return new CoreSize(ChartLegend.DesiredSize.Width,
                ChartLegend.DesiredSize.Height);
        }

        public List<AxisCore> MapXAxes(ChartCore chart)
        {
            if (AxisX.Count == 0) AxisX.AddRange(DefaultAxes.CleanAxis);
            return AxisX.Select(x =>
            {
                if (x.Parent == null) chart.View.AddToView(x);
                return x.AsCoreElement(Model, AxisTags.X);
            }).ToList();
        }

        public List<AxisCore> MapYAxes(ChartCore chart)
        {
            if (AxisY.Count == 0) AxisY.AddRange(DefaultAxes.DefaultAxis);
            return AxisY.Select(x =>
            {
                if (x.Parent == null) chart.View.AddToView(x);
                return x.AsCoreElement(Model, AxisTags.Y);
            }).ToList();
        }
    }
}
