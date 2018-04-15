namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The stacked bar interface.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.DataSeries.IBarSeries" />
    public interface IStackedBarSeries : IBarSeries
    {
        /// <summary>
        /// Gets or sets the index of the stack.
        /// </summary>
        /// <value>
        /// The index of the stack.
        /// </value>
        int StackIndex { get; set; }
    }
}