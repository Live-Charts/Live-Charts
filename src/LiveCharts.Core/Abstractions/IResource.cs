using System.ComponentModel;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Disposing resource delegate.
    /// </summary>
    /// <param name="view">The view.</param>
    public delegate void DisposingResourceHandler(IChartView view, object instance);

    /// <summary>
    /// A resource able to erase itself from memory and/or a chart view.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Occurs when the resource is disposed.
        /// </summary>
        event DisposingResourceHandler Disposed;

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