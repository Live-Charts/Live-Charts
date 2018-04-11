using System;
using LiveCharts.Core.Charts;

namespace LiveCharts.Core.DataSeries.Data
{
    /// <summary>
    /// An abstraction of the data factory context.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataFactoryContext : IDisposable
    {
        /// <summary>
        /// Gets the index of the grouping.
        /// </summary>
        /// <value>
        /// The index of the grouping.
        /// </value>
        int SeriesGroupingIndex { get; }

        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        ChartModel Chart { get; }

        /// <summary>
        /// Gets the update context.
        /// </summary>
        /// <value>
        /// The update context.
        /// </value>
        UpdateContext UpdateContext { get; }
    }
}