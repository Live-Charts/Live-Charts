using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Data;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a data tool tip.
    /// </summary>
    /// <seealso cref="IResource" />
    public interface IDataToolTip
    {
        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>
        /// The selection mode.
        /// </value>
        TooltipSelectionMode SelectionMode { get; }

        /// <summary>
        /// Measures this instance with the selected points.
        /// </summary>
        /// <returns></returns>
        SizeF ShowAndMeasure(IEnumerable<PackedPoint> selected, IChartView chart);

        /// <summary>
        ///  Moves to the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="chart">The chart.</param>
        void Move(PointF location, IChartView chart);

        /// <summary>
        /// Hides from specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void Hide(IChartView chart);
    }
}