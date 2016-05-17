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
using System.Windows;
using LiveCharts.Charts;
using LiveCharts.Wpf.Components.Chart;

// ReSharper disable once CheckNamespace
namespace LiveCharts.Wpf
{
    public class PieChart : Chart, IPieChart
    {
        public PieChart()
        {
            var freq = DisableAnimations ? TimeSpan.FromMilliseconds(10) : AnimationsSpeed;
            var updater = new Components.ChartUpdater(freq);
            ChartCoreModel = new PieChartCore(this, updater);
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof (double), typeof (PieChart), new PropertyMetadata(default(double)));

        public double InnerRadius
        {
            get { return (double) GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }
    }
}
