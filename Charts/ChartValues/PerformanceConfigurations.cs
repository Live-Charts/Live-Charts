using System;
using System.Collections.Generic;
using System.Linq;
using lvc;

namespace LiveCharts
{
    public static class PerformanceConfigurations
    {
        public static Func<ChartValues<T>, IEnumerable<T>> IndexedLineConfiguration<T>()
        {
            return x => x.AsEnumerable();
        }
    }
}
