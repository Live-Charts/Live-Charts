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

using System.Windows;
using System.Windows.Controls;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    /// <summary>
    /// Dont repeat yourself!
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static double ToPlotArea(double value, AxisTags axis, Chart chart)
        {
            //y = m (x - x1) + y1
            var p1 = axis == AxisTags.X
                ? new Point(chart.Max.X, chart.PlotArea.Width + chart.PlotArea.X)
                : new Point(chart.Max.Y, chart.PlotArea.Y);
            var p2 = axis == AxisTags.X
                ? new Point(chart.Min.X, chart.PlotArea.X)
                : new Point(chart.Min.Y, chart.PlotArea.Y + chart.PlotArea.Height);
            var m = (p2.Y - p1.Y) / (p2.X - p1.X);
            return m * (value - p1.X) + p1.Y;
        }

        public static double FromPlotArea(double value, AxisTags axis, Chart chart)
        {
            var p1 = axis == AxisTags.X
                ? new Point(chart.Max.X, chart.PlotArea.Width + chart.PlotArea.X)
                : new Point(chart.Max.Y, chart.PlotArea.Y);
            var p2 = axis == AxisTags.X
                ? new Point(chart.Min.X, chart.PlotArea.X)
                : new Point(chart.Min.Y, chart.PlotArea.Y + chart.PlotArea.Height);
            var m = (p2.Y - p1.Y) / (p2.X - p1.X);
            return (value + m*p1.X - p1.Y)/m;
        }

        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static Point ToPlotArea(Point value, Chart chart)
        {
            return new Point(ToPlotArea(value.X, AxisTags.X, chart), ToPlotArea(value.Y, AxisTags.Y, chart));
        }

        public static double ToDrawMargin(double value, AxisTags axis, Chart chart)
        {
            var o = axis == AxisTags.X ? chart.PlotArea.X : chart.PlotArea.Y;
            var of = axis == AxisTags.X ? chart.XOffset : chart.YOffset;

            return ToPlotArea(value, axis, chart) - o + of;
        }

        public static double FromDrawMargin(double value, AxisTags axis, Chart chart)
        {
            var o = axis == AxisTags.X ? chart.PlotArea.X : chart.PlotArea.Y;
            var of = axis == AxisTags.X ? chart.XOffset : chart.YOffset;

            return FromPlotArea(value, axis, chart) - o + of;
        }
    }
}