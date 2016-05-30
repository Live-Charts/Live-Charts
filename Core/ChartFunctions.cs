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
using LiveCharts.Charts;

namespace LiveCharts
{
    public static class ChartFunctions
    {
        private static readonly double Day = TimeSpan.FromDays(1).Ticks;

        /// <summary>
        /// Scales a chart value to screen value according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="chart"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static double ToPlotArea(double value, AxisTags source, ChartCore chart, int axis = 0)
        {
            var p1 = new CorePoint();
            var p2 = new CorePoint();

            if (source == AxisTags.Y)
            {
                p1.X = chart.AxisY[axis].MaxLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = chart.AxisY[axis].MinLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                p1.X = chart.AxisX[axis].MaxLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = chart.AxisX[axis].MinLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y)/(deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        public static double ToPlotArea(double value, AxisTags source, ChartCore chart, AxisCore axis)
        {
            var p1 = new CorePoint();
            var p2 = new CorePoint();

            if (source == AxisTags.Y)
            {
                p1.X = axis.MaxLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = axis.MinLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                p1.X = axis.MaxLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = axis.MinLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        public static CorePoint ToPlotArea(CorePoint point, ChartCore chart, int axis = 0)
        {
            return new CorePoint(
                ToPlotArea(point.X, AxisTags.X, chart, axis),
                ToPlotArea(point.Y, AxisTags.Y, chart, axis));
        }

        /// <summary>
        /// Scales a screen value to chart value accoring to an axis.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="chart"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static double FromPlotArea(double value, AxisTags source, ChartCore chart, int axis = 0)
        {
            var p1 = new CorePoint();
            var p2 = new CorePoint();
            
            if (source == AxisTags.Y)
            {
                p1.X = chart.AxisY[axis].MaxLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = chart.AxisY[axis].MinLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                p1.X = chart.AxisX[axis].MaxLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = chart.AxisX[axis].MinLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return (value + m * p1.X - p1.Y) / m;
        }
        
        public static double ToDrawMargin(double value, AxisTags source, ChartCore chart, int axis = 0)
        {
            var o = source == AxisTags.X
                ? chart.DrawMargin.Left
                : chart.DrawMargin.Top;

            return ToPlotArea(value, source, chart, axis) - o;
        }

        public static double ToDrawMargin(double value, AxisTags source, ChartCore chart, AxisCore axis)
        {
            var o = source == AxisTags.X
                ? chart.DrawMargin.Left
                : chart.DrawMargin.Top;

            return ToPlotArea(value, source, chart, axis) - o;
        }

        public static CorePoint ToDrawMargin(ChartPoint point, int axisXIndex, int axisYIndex, ChartCore chart)
        {
            //Disabled for now, instead each axis will have a unitary width according to the series it holds!
            //var unitaryOffset = chart.HasUnitaryPoints
            //    ? (chart.Invert
            //        ? new LvcPoint(0, GetUnitWidth(AxisTags.Y, chart, axisYIndex)*.5)
            //        : new LvcPoint(GetUnitWidth(AxisTags.X, chart, axisXIndex)*.5, 0))
            //    : new LvcPoint();
           
            return new CorePoint(
                ToDrawMargin(point.X, AxisTags.X, chart, axisXIndex),
                ToDrawMargin(point.Y, AxisTags.Y, chart, axisYIndex)); //+ unitaryOffset;
        }


        public static double FromDrawMargin(double value, AxisTags source, ChartCore chart, int axis = 0)
        {
            var o = source == AxisTags.X
                ? chart.DrawMargin.Left
                : chart.DrawMargin.Top;

            //var of = axis == AxisTags.X ? chart.XOffset : chart.YOffset;

            return FromPlotArea(value, source, chart, axis); //- o; //FromPlotArea(value, axis, chart) - o + of;
        }

        public static double GetUnitWidth(AxisTags source, ChartCore chart, int axis = 0)
        {
            double min;
            
            if (source == AxisTags.Y)
            {
                min = chart.AxisY[axis].MinLimit;
                return ToDrawMargin(min, AxisTags.Y, chart, axis) - ToDrawMargin(min + 1, AxisTags.Y, chart, axis);
            }

            min = chart.AxisX[axis].MinLimit;
            return ToDrawMargin(min + 1, AxisTags.X, chart, axis) - ToDrawMargin(min, AxisTags.X, chart, axis);
        }

        public static double GetUnitWidth(AxisTags source, ChartCore chart, AxisCore axis)
        {
            double min;

            if (source == AxisTags.Y)
            {
                min = axis.MinLimit;
                return ToDrawMargin(min, AxisTags.Y, chart, axis) - ToDrawMargin(min + 1, AxisTags.Y, chart, axis);
            }

            min = axis.MinLimit;
            return ToDrawMargin(min + 1, AxisTags.X, chart, axis) - ToDrawMargin(min, AxisTags.X, chart, axis);
        }

    }

    public enum AxisTags
    {
        X, Y, None
    }

    public static class AxisExtentions
    {
        public static AxisTags Invert(this AxisTags axis)
        {
            return axis == AxisTags.X ? AxisTags.Y : AxisTags.X;
        }
    }
}
 