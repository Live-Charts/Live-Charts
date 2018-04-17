namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The scatter series interface. 
    /// </summary>
    public interface IScatterSeries : ICartesianSeries, IStrokeSeries
    {
        /// <summary>
        /// Gets or sets the size of the <see cref="ISeries.Geometry"/> property. 
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        double GeometrySize { get; set; }
    }
}