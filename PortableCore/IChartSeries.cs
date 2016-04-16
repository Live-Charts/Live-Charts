namespace LiveChartsCore
{
    public interface IChartSeries
    {
        /// <summary>
        /// Gets or sets series values to plot.
        /// </summary>
        IChartValues Values { get; set; }
        /// <summary>
        /// Collection that owns the series.
        /// </summary>
        SeriesCollection Collection { get; }
        /// <summary>
        /// Gets or sets Series Configuration
        /// </summary>
        SeriesConfiguration Configuration { get; set; }
    }
}