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

using System.Collections.Generic;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public static class LiveChartsExtentions
    {
        public static ChartValues<T> AsChartValues<T>(this IEnumerable<T> values)
        {
            var l = new ChartValues<T>();
            l.AddRange(values);
            return l;
        }

        //based on http://stackoverflow.com/questions/13194898/linq-operator-to-split-list-of-doubles-to-multiple-list-of-double-based-on-gener
        internal static IEnumerable<List<ChartPoint>> AsSegments(this IEnumerable<ChartPoint> source)
        {
            var buffer = new List<ChartPoint>();
            foreach (var item in source)
            {
                if (double.IsNaN(item.Y) || double.IsNaN(item.X))
                {
                    yield return buffer;
                    buffer = new List<ChartPoint>();
                }
                else
                {
                    buffer.Add(item);
                }
            }
            yield return buffer;
        }
    }
}