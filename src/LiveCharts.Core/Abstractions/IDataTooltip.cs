using System.Collections.Generic;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a data tool tip.
    /// </summary>
    /// <seealso cref="IResource" />
    public interface IDataTooltip : IResource
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
        Size Measure(IEnumerable<PackedPoint> selected);

        /// <summary>
        /// Moves the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        void Move(Point location);
    }
}