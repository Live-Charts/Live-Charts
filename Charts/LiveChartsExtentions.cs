using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace lvc
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
        internal static IEnumerable<List<Point>> AsSegments(this IEnumerable<Point> source)
        {
            List<Point> buffer = new List<Point>();
            foreach (var item in source)
            {
                if (double.IsNaN(item.Y) || double.IsNaN(item.X))
                {
                    yield return buffer;
                    buffer = new List<Point>();
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