using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Events
{
    /// <summary>
    /// Disposing resource delegate.
    /// </summary>
    /// <param name="view">The view.</param>
    /// <param name="instance">The sender instance.</param>
    public delegate void DisposingResourceHandler(IChartView view, object instance);
}