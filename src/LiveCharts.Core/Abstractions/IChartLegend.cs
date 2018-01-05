using System.Collections.Generic;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart legend.
    /// </summary>
    public interface IChartLegend
    {
        /// <summary>
        /// Gets the size of the control.
        /// </summary>
        /// <value>
        /// The size of the control.
        /// </value>
        Size ControlSize { get; }

        /// <summary>
        /// Updates the layout.
        /// </summary>
        void UpdateLayout();

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        LegendPositions Position { get; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        IEnumerable<IChartSeries> Series { set; }
    }
}