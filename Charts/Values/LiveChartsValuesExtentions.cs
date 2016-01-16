using System.Collections.Generic;

namespace lvc
{
    public static class LiveChartsValuesExtentions
    {
        public static void AddRange(this IndexedChartValues chartValues, IEnumerable<double> toAdd)
        {
            foreach(var element in toAdd) chartValues.Add(element);
        }

        public static void AddRange<T>(this SeriesValues<T> values, IEnumerable<T> toAdd) where T : class
        {
            foreach (var element in toAdd) values.Add(element);
        }

        public static IndexedChartValues AsChartValues(this IEnumerable<double> values)
        {
            var l = new IndexedChartValues();
            l.AddRange(values);
            return l;
        }
    }
}