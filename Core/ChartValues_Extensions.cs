//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Configurations;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;
#if NET45
using System.Reflection;
#endif

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public static class IChartValues_Extensions
    {
        /// <summary>
        /// enumerate chart points that are in the rectangele or connected to them.
        /// </summary>
        /// <param name="chartValues"></param>
        /// <param name="seriesView"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static IEnumerable<ChartPoint> GetPoints(
            this IChartValues chartValues,
            ISeriesView seriesView, CoreRectangle rect)
        {

            ChartPoint previous = null;
            bool previousIsIn = false;

            var seriesOrientation = seriesView.Model?.SeriesOrientation ?? SeriesOrientation.Horizontal;

            foreach (var point in chartValues.GetPoints(seriesView))
            {

                //check the point if it is in the rectangle or not
                bool isIn = false;
                if (seriesOrientation == SeriesOrientation.Horizontal)
                {
                    isIn = (rect.Left <= point.ChartLocation.X) && (point.ChartLocation.X <= rect.Left + rect.Width);

                }
                else if (seriesOrientation == SeriesOrientation.Vertical)
                {
                    isIn = (rect.Top <= point.ChartLocation.Y) && (point.ChartLocation.Y <= rect.Top + rect.Height);

                }
                else
                {
                    isIn = (rect.Left <= point.ChartLocation.X)
                        && (point.ChartLocation.X <= rect.Left + rect.Width)
                        && (rect.Top <= point.ChartLocation.Y)
                        && (point.ChartLocation.Y <= rect.Top + rect.Height);
                }


                if (isIn)
                {
                    if (!previousIsIn && previous != null)
                    {
                        yield return previous;
                    }

                    previousIsIn = true;
                    yield return point;
                }
                else
                {
                    if (previousIsIn)
                    {
                        yield return point;
                    }

                    previousIsIn = false;
                }

                previous = point;
            }
        }
    }
}
