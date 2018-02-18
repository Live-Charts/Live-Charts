namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a cartesian chart view.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.IChartView" />
    public interface ICartesianChartView : IChartView
    {
        /// <summary>
        /// Gets or sets a value indicating whether the X/Y axis are inverted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [inverted axis]; otherwise, <c>false</c>.
        /// </value>
        bool InvertedAxis { get; set; }
    }
}