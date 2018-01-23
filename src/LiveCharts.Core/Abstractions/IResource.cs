namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// A resource able to erase itself from memory and/or a chart view.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Gets the update identifier.
        /// </summary>
        /// <value>
        /// The update identifier.
        /// </value>
        object UpdateId { get; set; }

        /// <summary>
        /// Disposes from specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        void Dispose(IChartView view);
    }
}