namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The column series definition.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.DataSeries.ISeries" />
    public interface IColumnSeries : ISeries
    {
        /// <summary>
        /// Gets or sets the column padding.
        /// </summary>
        /// <value>
        /// The column padding.
        /// </value>
        double ColumnPadding { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the column.
        /// </summary>
        /// <value>
        /// The maximum width of the column.
        /// </value>
        double MaxColumnWidth { get; set; }
    }
}