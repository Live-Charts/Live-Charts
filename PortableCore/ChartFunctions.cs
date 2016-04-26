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
using System.Collections;
using System.Collections.Generic;

namespace LiveChartsCore
{
    public static class ChartFunctions
    {
        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="chart"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static double ToPlotArea(double value, AxisTags source, IChartModel chart, int axis = 0)
        {
            //y = m * (x - x1) + y1

            var p1 = new LvcPoint();
            var p2 = new LvcPoint();
            
            if (source == AxisTags.Y)
            {
                var y = chart.AxisY as IList;
                if (y == null) return 0;
                var view = y[axis] as IAxisView;
                if (view == null) return 0;

                var ax = view.Model;

                p1.X = ax.MaxLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = ax.MinLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                var x = chart.AxisX as IList;
                if (x == null) return 0;
                var view = x[axis] as IAxisView;
                if (view == null) return 0;

                var ax =  view.Model;

                p1.X = ax.MaxLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = ax.MinLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        public static double FromPlotArea(double value, AxisTags source, IChartModel chart, int axis = 0)
        {
            var p1 = new LvcPoint();
            var p2 = new LvcPoint();


            if (source == AxisTags.Y)
            {
                var y = chart.AxisY as IList;
                if (y == null) return 0;
                var view = y[axis] as IAxisView;
                if (view == null) return 0;

                var ax = view.Model; 

                p1.X = ax.MaxLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = ax.MinLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                var x = chart.AxisX as IList;
                if (x == null) return 0;
                var view = x[axis] as IAxisView;
                if (view == null) return 0;

                var ax = view.Model;

                p1.X = ax.MaxLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = ax.MinLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return (value + m * p1.X - p1.Y) / m;
        }

        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="chart"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static LvcPoint ToPlotArea(LvcPoint point, IChartModel chart, int axis = 0)
        {
            return new LvcPoint(
                ToPlotArea(point.X, AxisTags.X, chart, axis),
                ToPlotArea(point.Y, AxisTags.Y, chart, axis));
        }

        public static double ToDrawMargin(double value, AxisTags source, IChartModel chart, int axis = 0)
        {
            var o = source == AxisTags.X
                ? chart.DrawMargin.Left
                : chart.DrawMargin.Top;

            return ToPlotArea(value, source, chart, axis) - o;
        }

        public static LvcPoint ToDrawMargin(ChartPoint point, int axisXIndex, int axisYIndex, IChartModel chart)
        {
            var unitaryOffset = chart.HasUnitaryPoints
                ? (chart.Invert
                    ? new LvcPoint(0, GetUnitWidth(AxisTags.Y, chart, axisYIndex)*.5)
                    : new LvcPoint(GetUnitWidth(AxisTags.X, chart, axisXIndex)*.5, 0))
                : new LvcPoint();

            return new LvcPoint(
                ToDrawMargin(point.X, AxisTags.X, chart, axisXIndex),
                ToDrawMargin(point.Y, AxisTags.Y, chart, axisYIndex))
                   + unitaryOffset;
        }

        public static double FromDrawMargin(double value, AxisTags source, IChartModel chart, int axis = 0)
        {
            //var o = axis == AxisTags.X
            //    ? Canvas.GetLeft(chart.DrawMargin)
            //    : Canvas.GetTop(chart.DrawMargin);
            //var of = axis == AxisTags.X ? chart.XOffset : chart.YOffset;

            return FromPlotArea(value, source, chart, axis);//FromPlotArea(value, axis, chart) - o + of;
        }

        public static double GetUnitWidth(AxisTags source, IChartModel chart, int axis = 0)
        {
            double min;

            var x = chart.AxisX as List<IAxisView>;
            var y = chart.AxisY as List<IAxisView>;

            if (x == null || y == null) return 0;

            if (source == AxisTags.Y)
            {
                min = y[axis].Model.MinLimit;
                return ToDrawMargin(min, AxisTags.Y, chart, axis) - ToDrawMargin(min + 1, AxisTags.Y, chart, axis);
            }

            min = x[axis].Model.MinLimit;
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
