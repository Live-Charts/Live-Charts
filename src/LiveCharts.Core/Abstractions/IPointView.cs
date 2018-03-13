using System.Drawing;
using LiveCharts.Core.Data;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">The type of the chart point.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the point model.</typeparam>
    public interface IPointView<TModel, in TPoint, TCoordinate, TViewModel> : IResource
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
        IDataLabelControl Label { get; }

        /// <summary>
        /// Draws the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="previous">The previous.</param>
        void DrawShape(TPoint point, TPoint previous);

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="location">The location.</param>
        void DrawLabel(TPoint point, PointF location);
    }
}
