namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The pie series interface.
    /// </summary>
    public interface IPieSeries: IStrokeSeries
    {
        /// <summary>
        /// Gets or sets the push out.
        /// </summary>
        /// <value>
        /// The push out.
        /// </value>
        double PushOut { get; set; }

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        double CornerRadius { get; set; }
    }
}