using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// The model state event arguments.
    /// </summary>
    public class ModelStateEventArgs<TModel, TCoordinate>
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelStateEventArgs{TModel, TCoordinate}"/> class.
        /// </summary>
        /// <param name="visualElement">The visual.</param>
        /// <param name="point">the point.</param>
        public ModelStateEventArgs(
            object visualElement,
            Point<TModel, TCoordinate, object> point)
        {
            VisualElement = visualElement;
            Point = point;
        }

        /// <summary>
        /// Gets a copy of the chart point, this copy has no access to the point view.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public Point<TModel, TCoordinate, object> Point { get; }

        /// <summary>
        /// Gets the visual.
        /// </summary>
        /// <value>
        /// The visual.
        /// </value>
        public object VisualElement { get; }
    }
}