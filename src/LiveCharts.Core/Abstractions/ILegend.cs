using System.Collections.Generic;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart legend.
    /// </summary>
    /// <seealso cref="IResource" />
    public interface ILegend : IResource
    {
        /// <summary>
        /// Measures and places this instance in the UI.
        /// </summary>
        /// <param name="seriesCollection">The series collection.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        float[] Measure(IEnumerable<DataSet> seriesCollection, Orientation orientation, IChartView chart);

        /// <summary>
        /// Moves to the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        void Move(Point location, IChartView chart);
    }
}