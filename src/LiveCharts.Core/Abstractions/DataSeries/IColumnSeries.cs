namespace LiveCharts.Core.Abstractions.DataSeries
{
    public interface IColumnSeries: ISeries
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

        /// <summary>
        /// Gets or sets the point corner radius.
        /// </summary>
        /// <value>
        /// The point corner radius.
        /// </value>
        double PointCornerRadius { get; set; }
}
}