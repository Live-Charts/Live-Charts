namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// A bi dimensional series.
    /// </summary>
    public interface ICartesianSeries : ISeries
    {
        /// <summary>
        /// Gets the scales at array, this property is used internally by the library and should only be used
        /// by you if you need to build a custom series.
        /// </summary>
        /// <value>
        /// The scales at.
        /// </value>
        int[] ScalesAt { get; }

        /// <summary>
        /// Gets or sets the z index, it is the position of the series in the Z axis.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        int ZIndex { get; set; }
    }
}