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

using System.Diagnostics;
using System.Windows;

namespace LiveCharts.Wpf.Charts.Chart
{
    public abstract partial class Chart
    {
        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            IsControlLoaded = true;

            Model.ChartControlSize = new CoreSize(ActualWidth, ActualHeight);

            Model.DrawMargin.Height = Canvas.ActualHeight;
            Model.DrawMargin.Width = Canvas.ActualWidth;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
#if DEBUG
            Debug.WriteLine("ChartResized");
#endif
            Model.ChartControlSize = new CoreSize(ActualWidth, ActualHeight);

            Model.Updater.Run();
        }

        private static void OnSeriesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var chart = (Chart) dependencyObject;

            if (chart.LastKnownSeriesCollection != chart.Series && chart.LastKnownSeriesCollection != null)
            {
                foreach (var series in chart.LastKnownSeriesCollection)
                {
                    series.Erase();
                }
            }

            CallChartUpdater()(dependencyObject, dependencyPropertyChangedEventArgs);
            chart.LastKnownSeriesCollection = chart.Series;
        }
    }
}
