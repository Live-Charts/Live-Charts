using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Defines a point that is able to store a chart point.
    /// </summary>
    public interface IChartPoint<TModel, TCoordinate, TViewModel, TPoint>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        TPoint ChartPoint { get; set; }
    }
}