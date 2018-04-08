namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// A bi dimensional series.
    /// </summary>
    public interface ICartesianSeries
    {
        /// <summary>
        /// Gets or sets the z index, it is the position of the series in the Z axis.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        int ZIndex { get; set; }
    }
}