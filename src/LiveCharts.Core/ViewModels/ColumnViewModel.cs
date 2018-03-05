namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// The column view model.
    /// </summary>
    public struct ColumnViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnViewModel"/> struct.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public ColumnViewModel(Column from, Column to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public Column From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public Column To { get; set; }
    }
}
