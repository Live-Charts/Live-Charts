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
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public static class Methods
    {
        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="chart"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static double ToPlotArea(double value, AxisTags source, Chart chart, int axis = 0)
        {
            //y = m * (x - x1) + y1

            var p1 = new Point();
            var p2 = new Point();

            if (source == AxisTags.Y)
            {
                if (axis >= chart.AxisY.Count)
                    throw new Exception("There is not a valid Y axis at position " + axis);

                var ax = chart.AxisY[axis];

                p1.X = ax.MaxLimit;
                p1.Y = chart.PlotArea.Y;

                p2.X = ax.MinLimit;
                p2.Y = chart.PlotArea.Y + chart.PlotArea.Height;
            }
            else
            {
                if (axis >= chart.AxisX.Count)
                    throw new Exception("There is not a valid X axis at position " + axis);

                var ax = chart.AxisX[axis];

                p1.X = ax.MaxLimit;
                p1.Y = chart.PlotArea.Width + chart.PlotArea.X;

                p2.X = ax.MinLimit;
                p2.Y = chart.PlotArea.X;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y)/(deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        public static double FromPlotArea(double value, AxisTags source, Chart chart, int axis = 0)
        {
            var p1 = new Point();
            var p2 = new Point();

            if (source == AxisTags.Y)
            {
                if (axis >= chart.AxisY.Count)
                    throw new Exception("There is not a valid Y axis at position " + axis);

                var ax = chart.AxisY[axis];

                p1.X = ax.MaxLimit;
                p1.Y = chart.PlotArea.Y;

                p2.X = ax.MinLimit;
                p2.Y = chart.PlotArea.Y + chart.PlotArea.Height;
            }
            else
            {
                if (axis >= chart.AxisX.Count)
                    throw new Exception("There is not a valid X axis at position " + axis);

                var ax = chart.AxisX[axis];

                p1.X = ax.MaxLimit;
                p1.Y = chart.PlotArea.Width + chart.PlotArea.X;

                p2.X = ax.MinLimit;
                p2.Y = chart.PlotArea.X;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y)/(deltaX == 0 ? double.MinValue : deltaX);
            return (value + m*p1.X - p1.Y)/m;
        }

        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chart"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Point ToPlotArea(Point value, Chart chart, int axis = 0)
        {
            return new Point(
                ToPlotArea(value.X, AxisTags.X, chart, axis),
                ToPlotArea(value.Y, AxisTags.Y, chart, axis));
        }

        public static double ToDrawMargin(double value, AxisTags source, Chart chart, int axis = 0)
        {
            var o = source == AxisTags.X ? chart.PlotArea.X : chart.PlotArea.Y;
            var of = source == AxisTags.X ? chart.XOffset : chart.YOffset;

            return ToPlotArea(value, source, chart, axis) - o + of;
        }

        public static double FromDrawMargin(double value, AxisTags axis, Chart chart)
        {
            var o = axis == AxisTags.X ? chart.PlotArea.X : chart.PlotArea.Y;
            var of = axis == AxisTags.X ? chart.XOffset : chart.YOffset;

            return FromPlotArea(value, axis, chart) - o + of;
        }
    }
}