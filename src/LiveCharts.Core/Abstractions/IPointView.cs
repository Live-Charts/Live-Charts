namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart point view.
    /// </summary>
    /// <seealso cref="IDisposableChartResource" />
    public interface IPointView
    {
        /// <summary>
        /// Erases this instance.
        /// </summary>
        void Erase();
    }
}