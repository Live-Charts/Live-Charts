using System.Collections.Generic;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart legend.
    /// </summary>
    public interface ILegend : IResource
    {
        /// <summary>
        /// Measures this instance.
        /// </summary>
        /// <returns></returns>
        Size Measure(IEnumerable<Series> seriesCollection, Orientation orientation);
    }
}