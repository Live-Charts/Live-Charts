using System.Collections.Generic;

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