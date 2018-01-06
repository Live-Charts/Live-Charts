using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart point view.
    /// </summary>
    public interface IPointView
    {
        /// <summary>
        /// Draws the specified point.
        /// </summary>
        /// <param name="point">The point to draw.</param>
        /// <param name="previous">The previous point.</param>
        /// <param name="chart">The chart.</param>
        void Draw(ChartPoint point, ChartPoint previous, IChartView chart);

        /// <summary>
        /// Erases this instance.
        /// </summary>
        void Erase();
    }
}