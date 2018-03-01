using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The scatter series interface.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.DataSeries.ISeries" />
    public interface IScatterSeries : ISeries
    {
        /// <summary>
        /// Gets or sets the point geometry.
        /// </summary>
        /// <value>
        /// The point geometry.
        /// </value>
        Geometry PointGeometry { get; set; }

        /// <summary>
        /// Gets or sets the maximum point diameter.
        /// </summary>
        /// <value>
        /// The maximum point diameter.
        /// </value>
        float MaxPointDiameter { get; set; }

        /// <summary>
        /// Gets or sets the minimum point diameter.
        /// </summary>
        /// <value>
        /// The minimum point diameter.
        /// </value>
        float MinPointDiameter { get; set; }
    }
}