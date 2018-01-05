using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines that an object is able to build a chart point.
    /// </summary>
    public interface IChartingPointBuilder
    {
        /// <summary>
        /// Builds a <see cref="ChartPoint"/> with a given instance and key.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="index">The key.</param>
        /// <returns></returns>
        ChartPoint Build(object instance, int index);
    }
}