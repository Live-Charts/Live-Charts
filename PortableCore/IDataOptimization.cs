using System;
using System.Collections.Generic;

namespace LiveChartsCore
{
    public interface IDataOptimization<T>
    {
        Func<T, int, double> XMapper { get; set; }
        Func<T, int, double> YMapper { get; set; }
        IEnumerable<ChartPoint> Run(IEnumerable<KeyValuePair<int, T>> data);
    }
}