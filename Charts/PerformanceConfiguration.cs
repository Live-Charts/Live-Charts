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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts.Charts;

namespace LiveCharts
{
    public class PerformanceConfiguration
    {
        public PerformanceConfiguration()
        {
            Enabled = true;
            PixelsPerPoint = 10;
        }
        /// <summary>
        /// Gets or sets if a chart enables performance optimization
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the number of pixels per point, grether = faster, smaller = High Definition
        /// </summary>
        public int PixelsPerPoint { get; set; }
    }

    public static class PerformanceConfigurationExtentions
    {
        public static List<Point> OptimizeAsIndexedChart(this IEnumerable<Point> points, Chart chart)
        {
            var isFirst = true;
            var result = new List<Point>();
            var ppp = chart.PerformanceConfiguration.PixelsPerPoint;
            double? x = null;
            var g = new List<Point>();
            foreach (var point in points)
            {
                var chartValue = chart.ToPlotArea(point.X,AxisTags.X);
                if (x == null) x = chartValue;
                if (chartValue - x < ppp)
                {
                    g.Add(point);
                    continue;
                }
                //ToDo: Think about this:
                //average seems the best "general" method, but maybe a developer
                //should be able to choose the method.
                var xx = g.Average(p => p.X);
                if (isFirst)
                {
                    xx = g.First().X;
                    isFirst = false;
                }
                result.Add(new Point(xx, g.Average(p => p.Y)));
                g = new List<Point> {point};
                x = chart.ToPlotArea(point.X, AxisTags.X);
            }
            result.Add(new Point(g.Last().X, g.Average(p => p.Y)));
            return result;
        }
    }
}
