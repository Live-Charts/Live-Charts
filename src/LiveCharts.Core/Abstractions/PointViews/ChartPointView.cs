using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions.PointViews
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TChartPoint">The type of the chart point.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the point model.</typeparam>
    public abstract class ChartPointView<TModel, TChartPoint, TCoordinate, TViewModel>
        where TChartPoint : ChartPoint<TModel, TCoordinate, TViewModel>, new()
    {
        /// <summary>
        /// Draws the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="model">The model.</param>
        /// <param name="previous">The previous.</param>
        /// <param name="chart">The chart.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract void Draw(TChartPoint point, TViewModel model, TChartPoint previous, IChartView chart);

        /// <summary>
        /// Erases the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract void Erase(IChartView chart);
    }
}
