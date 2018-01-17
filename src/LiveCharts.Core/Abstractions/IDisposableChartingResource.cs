namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// A resource able to erase itself from memory and/or a chart view.
    /// </summary>
    public interface IDisposableChartingResource
    {
        void Dispose(IChartView view);
    }
}