namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a cartesian chart view.
    /// </summary>
    /// <seealso cref="IChartView" />
    public interface ICartesianChartView : IChartView
    {
        /// <summary>
        /// Gets or sets a value indicating whether the X/Y axis are inverted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the axis is inverted; otherwise, <c>false</c>, default is false.
        /// </value>
        bool InvertAxis { get; set; }
    }
}