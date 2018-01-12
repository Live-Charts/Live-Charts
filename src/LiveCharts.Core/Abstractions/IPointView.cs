using LiveCharts.Core.Data;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">The type of the chart point.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the point model.</typeparam>
    public interface IPointView<TModel, in TPoint, TCoordinate, in TViewModel>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Gets the visual element.
        /// </summary>
        /// <value>
        /// The visual.
        /// </value>
        object VisualElement { get; }

        /// <summary>
        /// Draws the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="model">The model.</param>
        /// <param name="previous">The previous.</param>
        /// <param name="chart">The chart.</param>
        void Draw(TPoint point, TPoint previous, IChartView chart, TViewModel model);

        /// <summary>
        /// Erases the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void Erase(IChartView chart);
    }
}
