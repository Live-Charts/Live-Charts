using System;
using System.Collections.Generic;

namespace lvc
{
    public static class LiveChartsValuesExtentions
    {
        public static ChartValues<T> AsChartValues<T>(this IEnumerable<T> values, Func<T, int, double> x, Func<T, int, double> y)
        {
            var l = new ChartValues<T>().PullX(x).PullY(y);
            l.AddRange(values);
            return l;
        }
        public static ChartValues<T> AsChartValues<T>(this IEnumerable<T> values, Func<T, double> x, Func<T, double> y)
        {
            return values.AsChartValues((val, i) => x(val), (val, i) => y(val));
        }
        public static ChartValues<T> AsChartValues<T>(this IEnumerable<T> values, Func<T, int, double> x, Func<T, double> y)
        {
            return values.AsChartValues(x, (val, i) => y(val));
        }
        public static ChartValues<T> AsChartValues<T>(this IEnumerable<T> values, Func<T, double> x, Func<T, int, double> y)
        {
            return values.AsChartValues((val, i) => x(val), y);
        }
    }
}