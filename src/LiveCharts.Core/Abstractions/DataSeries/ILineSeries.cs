using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The line series interface.
    /// </summary>
    public interface ILineSeries : ISeries
    {
        /// <summary>
        /// Gets or sets the line smoothness.
        /// </summary>
        /// <value>
        /// The line smoothness.
        /// </value>
        double LineSmoothness { get; set; }

        /// <summary>
        /// Gets or sets the point geometry.
        /// </summary>
        /// <value>
        /// The point geometry.
        /// </value>
        Geometry PointGeometry { get; set; }

        /// <summary>
        /// Gets or sets the size of the geometry.
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        double GeometrySize { get; set; }
    }
}