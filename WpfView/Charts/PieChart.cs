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
using LiveCharts.Wpf.Charts.Chart;
using LiveCharts.Wpf.Points;

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
            "InnerRadius", typeof (double), typeof (PieChart), new PropertyMetadata(0d, CallChartUpdater()));

        public double InnerRadius
        {
            get { return (double) GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty StartingRotationAngleProperty = DependencyProperty.Register(
            "StartingRotationAngle", typeof (double), typeof (PieChart), new PropertyMetadata(45d, CallChartUpdater()));

        public double StartingRotationAngle
        {
            get { return (double) GetValue(StartingRotationAngleProperty); }
            set { SetValue(StartingRotationAngleProperty, value); }
        }

        protected override Point GetTooltipPosition(ChartPoint senderPoint)
        {
            var pieSlice = ((PiePointView) senderPoint.View).Slice;

            var alpha = pieSlice.RotationAngle + pieSlice.WedgeAngle * .5 + 180;
            var alphaRad = alpha * Math.PI / 180;
            var sliceMidAngle = alpha - 180;

            DataTooltip.UpdateLayout();

            var y = DrawMargin.ActualHeight*.5 +
                    (sliceMidAngle > 90 && sliceMidAngle < 270 ? -1 : 0)*DataTooltip.ActualHeight -
                    Math.Cos(alphaRad)*15;
            var x = DrawMargin.ActualWidth*.5 +
                    (sliceMidAngle > 0 && sliceMidAngle < 180 ? -1 : 0)*DataTooltip.ActualWidth +
                    Math.Sin(alphaRad)*15;

            return new Point(x, y);
        }
    }
}
