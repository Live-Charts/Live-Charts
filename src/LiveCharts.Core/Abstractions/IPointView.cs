using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">The type of the chart point.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the point model.</typeparam>
    public interface IPointView<TModel, in TPoint, TCoordinate, in TViewModel> : IResource
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
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        object Label { get; }

        /// <summary>
        /// Draws the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="viewModel">The model.</param>
        /// <param name="previous">The previous.</param>
        /// <param name="chart">The chart.</param>
        void DrawShape(TPoint point, TPoint previous, IChartView chart, TViewModel viewModel);

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="location">The location.</param>
        /// <param name="chart">The chart.</param>
        void DrawLabel(TPoint point, Point location, IChartView chart);
    }
}
