using System.Collections.Generic;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart legend.
    /// </summary>
    public interface ILegend : IDisposableChartingResource
    {
        /// <summary>
        /// Measures this instance.
        /// </summary>
        /// <returns></returns>
        Size Measure(IEnumerable<ISeries> seriesCollection, Orientation orientation);
    }
}