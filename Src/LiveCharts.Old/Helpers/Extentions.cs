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
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.Helpers
{
    /// <summary>
    /// LiveCharts extensions methods
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// Executes an Action in every item of a collection
        /// </summary>
        /// <typeparam name="T">type to iterate with</typeparam>
        /// <param name="source">collection to iterate</param>
        /// <param name="predicate">action to execute</param>
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T> predicate)
        {
            foreach (var item in source) predicate(item);
        }

        /// <summary>
        /// Splits a collection of points every double.Nan
        /// </summary>
        /// <param name="toSplit">collection to split</param>
        /// <returns>collection of collections</returns>
        internal static IEnumerable<IList<ChartPoint>> SplitEachNaN(this IList<ChartPoint> toSplit)
        {
            var l = new List<ChartPoint>(toSplit.Count);
            var acum = -1;

            foreach (var point in toSplit)
            {
                if (double.IsNaN(point.X) || double.IsNaN(point.Y))
                {
                    yield return l;
                    acum += l.Count;
                    l = new List<ChartPoint>(toSplit.Count - acum);
                }
                else
                {
                    l.Add(point);
                }
            }

            yield return l;
        }

        /// <summary>
        /// Return the inverse axis orientation
        /// </summary>
        /// <param name="axis">current orientation</param>
        /// <returns>inverted axis orientation</returns>
        internal static AxisOrientation Invert(this AxisOrientation axis)
        {
            return axis == AxisOrientation.X
                ? AxisOrientation.Y
                : AxisOrientation.X;
        }

        /// <summary>
        /// Converts any collection to chart values
        /// </summary>
        /// <typeparam name="T">type to convert</typeparam>
        /// <param name="values">values to convert</param>
        /// <returns>a new ChartValues instance containing the passed collection</returns>
        public static ChartValues<T> AsChartValues<T>(this IEnumerable<T> values)
        {
            var l = new ChartValues<T>();
            l.AddRange(values);
            return l;
        }

        /// <summary>
        /// Converts an enumeration of series to a SeriesCollection instance.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <returns></returns>
        public static SeriesCollection AsSeriesCollection(this IEnumerable<ISeriesView> series)
        {
            var collection = new SeriesCollection();
            collection.AddRange(series);
            return collection;
        }

        /// <summary>
        /// Gets the closest chart point with a given value.
        /// </summary>
        /// <param name="series">The target series.</param>
        /// <param name="value">The value.</param>
        /// <param name="orientation">the axis orientation</param>
        /// <returns></returns>
        public static ChartPoint ClosestPointTo(this ISeriesView series, double value, AxisOrientation orientation)
        {
            ChartPoint t = null;
            var delta = double.PositiveInfinity;

            foreach (var point in series.Values.GetPoints(series))
            {
                var i = orientation == AxisOrientation.X ? point.X : point.Y;

                var di = Math.Abs(i - value);

                if (di < delta)
                {
                    t = point;
                    delta = di;
                }
            }

            return t;
        }
    }
}
