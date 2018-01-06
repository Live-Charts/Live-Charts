using System.Collections.Generic;
using System.Threading.Tasks;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart legend.
    /// </summary>
    public interface IChartLegend
    {
        /// <summary>
        /// Removes the legend.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void RemoveLegend(IChartView chart);

        /// <summary>
        /// Updates the layout asynchronous.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="orientation">The orientation.</param>
        /// <returns></returns>
        Task<Size> UpdateLayoutAsync(IEnumerable<IChartSeries> series, Orientation orientation);
    }
}