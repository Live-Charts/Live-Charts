//MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files ("Software"), to deal
//in Software without restriction, including without limitation rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of Software, and to permit persons to whom Software is
//furnished to do so, subject to following conditions:

//above copyright notice and this permission notice shall be included in all
//copies or substantial portions of Software.

//SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH SOFTWARE OR USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;

namespace LiveCharts
{
    /// <summary>
    /// Contains useful methods to apply to a chart
    /// </summary>
    public static class ChartFunctions
    {
        /// <summary>
        /// Converts from chart values to chart control size.
        /// </summary>
        /// <param name="value">value to scale</param>
        /// <param name="source">axis orientation to scale value at</param>
        /// <param name="chart">chart model to scale value at</param>
        /// <param name="axis">axis index in collection of chart.axis</param>
        /// <returns></returns>
        public static double ToPlotArea(double value, AxisOrientation source, ChartCore chart, int axis = 0)
        {
            var p1 = new CorePoint();
            var p2 = new CorePoint();

            if (source == AxisOrientation.Y)
            {
                p1.X = chart.AxisY[axis].TopLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = chart.AxisY[axis].BotLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                p1.X = chart.AxisX[axis].TopLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = chart.AxisX[axis].BotLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y)/(deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        /// <summary>
        /// Converts from chart values to chart control size.
        /// </summary>
        /// <param name="value">value to scale</param>
        /// <param name="source">axis orientation to scale value at</param>
        /// <param name="chart">chart model to scale value at</param>
        /// <param name="axis">axis model instance</param>
        /// <returns></returns>
        public static double ToPlotArea(double value, AxisOrientation source, ChartCore chart, AxisCore axis)
        {
            var p1 = new CorePoint();
            var p2 = new CorePoint();

            if (source == AxisOrientation.Y)
            {
                p1.X = axis.TopLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = axis.BotLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                p1.X = axis.TopLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = axis.BotLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        /// <summary>
        /// Converts from chart control size to chart values.
        /// </summary>
        /// <param name="value">value to scale</param>
        /// <param name="source">axis orientation to scale value at</param>
        /// <param name="chart">chart model to scale value at</param>
        /// <param name="axis">axis index in collection of chart.axis</param>
        /// <returns></returns>
        public static double FromPlotArea(double value, AxisOrientation source, ChartCore chart, int axis = 0)
        {
            var p1 = new CorePoint();
            var p2 = new CorePoint();
            
            if (source == AxisOrientation.Y)
            {
                p1.X = chart.AxisY[axis].TopLimit;
                p1.Y = chart.DrawMargin.Top;

                p2.X = chart.AxisY[axis].BotLimit;
                p2.Y = chart.DrawMargin.Top + chart.DrawMargin.Height;
            }
            else
            {
                p1.X = chart.AxisX[axis].TopLimit;
                p1.Y = chart.DrawMargin.Width + chart.DrawMargin.Left;

                p2.X = chart.AxisX[axis].BotLimit;
                p2.Y = chart.DrawMargin.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return (value + m * p1.X - p1.Y) / m;
        }

        /// <summary>
        /// Converts from chart values to chart draw margin size.
        /// </summary>
        /// <param name="value">value to scale</param>
        /// <param name="source">axis orientation</param>
        /// <param name="chart">chart model to scale the value at</param>
        /// <param name="axis">axis instance to scale the value at</param>
        /// <returns></returns>
        public static double ToDrawMargin(double value, AxisOrientation source, ChartCore chart, int axis = 0)
        {
            var o = source == AxisOrientation.X
                ? chart.DrawMargin.Left
                : chart.DrawMargin.Top;

            return ToPlotArea(value, source, chart, axis) - o;
        }

        /// <summary>
        /// Converts from chart values to chart draw margin size.
        /// </summary>
        /// <param name="value">value to scale</param>
        /// <param name="source">axis orientation</param>
        /// <param name="chart">chart model to scale the value at</param>
        /// <param name="axis">axis instance to scale the value at</param>
        /// <returns></returns>
        public static double ToDrawMargin(double value, AxisOrientation source, ChartCore chart, AxisCore axis)
        {
            var o = source == AxisOrientation.X
                ? chart.DrawMargin.Left
                : chart.DrawMargin.Top;

            return ToPlotArea(value, source, chart, axis) - o;
        }

        /// <summary>
        /// Converts from chart values to chart draw margin size.
        /// </summary>
        /// <param name="point">point to scale</param>
        /// <param name="axisXIndex">axis orientation</param>
        /// <param name="axisYIndex">axis instance to scale the value at</param>
        /// <param name="chart">chart model to scale the value at</param>
        /// <returns></returns>
        public static CorePoint ToDrawMargin(ChartPoint point, int axisXIndex, int axisYIndex, ChartCore chart)
        {
            return new CorePoint(
                ToDrawMargin(point.X, AxisOrientation.X, chart, axisXIndex),
                ToDrawMargin(point.Y, AxisOrientation.Y, chart, axisYIndex));
        }

        /// <summary>
        /// Gets the width of a unit in the chart
        /// </summary>
        /// <param name="source">axis orientation</param>
        /// <param name="chart">chart model to get the scale at</param>
        /// <param name="axis">axis index in the axes collection</param>
        /// <returns></returns>
        public static double GetUnitWidth(AxisOrientation source, ChartCore chart, int axis = 0)
        {
            return GetUnitWidth(source, chart,
                (source == AxisOrientation.X ? chart.AxisX : chart.AxisY)[axis]);
        }

        /// <summary>
        /// Gets the width of a unit in the chart
        /// </summary>
        /// <param name="source">axis orientation</param>
        /// <param name="chart">chart model to get the scale at</param>
        /// <param name="axis">axis instance</param>
        /// <returns></returns>
        public static double GetUnitWidth(AxisOrientation source, ChartCore chart, AxisCore axis)
        {
            double min;
            double u = !double.IsNaN(axis.View.BarUnit)
                    ? axis.View.BarUnit
                    : (!double.IsNaN(axis.View.Unit)
                        ? axis.View.Unit
                        : 1);

            if (source == AxisOrientation.Y)
            {
                min = axis.BotLimit;
                return ToDrawMargin(min, AxisOrientation.Y, chart, axis) -
                       ToDrawMargin(min + u, AxisOrientation.Y, chart, axis);
            }

            min = axis.BotLimit;

            return ToDrawMargin(min + u, AxisOrientation.X, chart, axis) -
                   ToDrawMargin(min, AxisOrientation.X, chart, axis);
        }

        /// <summary>
        /// Returns data in the chart according to:
        /// </summary>
        /// <param name="senderPoint">point that was hovered</param>
        /// <param name="chart">chart model to get the data from</param>
        /// <param name="selectionMode">selection mode</param>
        /// <returns></returns>
        public static TooltipDataViewModel GetTooltipData(ChartPoint senderPoint, ChartCore chart, TooltipSelectionMode selectionMode)
        {
            var ax = chart.AxisX[senderPoint.SeriesView.ScalesXAt];
            var ay = chart.AxisY[senderPoint.SeriesView.ScalesYAt];

            switch (selectionMode)
            {
                case TooltipSelectionMode.OnlySender:
                    return new TooltipDataViewModel
                    {
                        XFormatter = ax.GetFormatter(),
                        YFormatter = ay.GetFormatter(),
                        Points = new List<ChartPoint> {senderPoint},
                        Shares = null
                    };
                case TooltipSelectionMode.SharedXValues:
                    var tx = Math.Abs(FromPlotArea(1, AxisOrientation.X, chart, senderPoint.SeriesView.ScalesXAt)
                                      - FromPlotArea(2, AxisOrientation.X, chart, senderPoint.SeriesView.ScalesXAt)) * .01;
                    return new TooltipDataViewModel
                    {
                        XFormatter = ax.GetFormatter(),
                        YFormatter = ay.GetFormatter(),
                        Points = chart.View.ActualSeries.Where(x => x.ScalesXAt == senderPoint.SeriesView.ScalesXAt)
                            .SelectMany(x => x.Values.GetPoints(x))
                            .Where(x => Math.Abs(x.X - senderPoint.X) < tx),
                        Shares = (chart.View is IPieChart) ? null : (double?) senderPoint.X
                    };
                case TooltipSelectionMode.SharedYValues:
                    var ty = Math.Abs(FromPlotArea(1, AxisOrientation.Y, chart, senderPoint.SeriesView.ScalesYAt)
                                     - FromPlotArea(2, AxisOrientation.Y, chart, senderPoint.SeriesView.ScalesYAt)) * .01;
                    return new TooltipDataViewModel
                    {
                        XFormatter = ax.GetFormatter(),
                        YFormatter = ay.GetFormatter(),
                        Points = chart.View.ActualSeries.Where(x => x.ScalesYAt == senderPoint.SeriesView.ScalesYAt)
                            .SelectMany(x => x.Values.GetPoints(x))
                            .Where(x => Math.Abs(x.Y - senderPoint.Y) < ty),
                        Shares = senderPoint.Y
                    };
                case TooltipSelectionMode.SharedXInSeries:
                    var txs = Math.Abs(FromPlotArea(1, AxisOrientation.X, chart, senderPoint.SeriesView.ScalesXAt)
                                     - FromPlotArea(2, AxisOrientation.X, chart, senderPoint.SeriesView.ScalesXAt)) * .01;
                    return new TooltipDataViewModel
                    {
                        XFormatter = ax.GetFormatter(),
                        YFormatter = ay.GetFormatter(),
                        Points = senderPoint.SeriesView.ActualValues.GetPoints(senderPoint.SeriesView)
                            .Where(x => Math.Abs(x.X - senderPoint.X) < txs),
                        Shares = senderPoint.X
                    };
                case TooltipSelectionMode.SharedYInSeries:
                    var tys = Math.Abs(FromPlotArea(1, AxisOrientation.Y, chart, senderPoint.SeriesView.ScalesYAt)
                                     - FromPlotArea(2, AxisOrientation.Y, chart, senderPoint.SeriesView.ScalesYAt)) * .01;
                    return new TooltipDataViewModel
                    {
                        XFormatter = ax.GetFormatter(),
                        YFormatter = ay.GetFormatter(),
                        Points = senderPoint.SeriesView.ActualValues.GetPoints(senderPoint.SeriesView)
                            .Where(x => Math.Abs(x.Y - senderPoint.Y) < tys),
                        Shares = senderPoint.Y
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
 