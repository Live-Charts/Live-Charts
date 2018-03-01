using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Events;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a separator view.
    /// </summary>
    public interface ICartesianAxisSeparator : IResource
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        object VisualElement { get; }

        /// <summary>
        /// Moves the specified point1.
        /// </summary>
        /// <param name="args">The arguments.</param>
        void Move(CartesianAxisSeparatorArgs args);
    }
}