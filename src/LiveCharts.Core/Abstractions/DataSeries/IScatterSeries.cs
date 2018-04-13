namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The scatter series interface. 
    /// </summary>
    public interface IScatterSeries : ISeries
    {
        /// <summary>
        /// Gets or sets the size of the <see cref="ISeries.Geometry"/> property. 
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        float GeometrySize { get; set; }
    }
}