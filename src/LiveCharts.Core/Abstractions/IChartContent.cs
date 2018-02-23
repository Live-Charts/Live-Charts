using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines the chart content view.
    /// </summary>
    public interface IChartContent
    {
        /// <summary>
        /// Gets or sets the draw area.
        /// </summary>
        /// <value>
        /// The draw area.
        /// </value>
        Rectangle DrawArea { get; set; }

        /// <summary>
        /// Adds a child child.
        /// </summary>
        void AddChild(object child);

        /// <summary>
        /// Moves the child.
        /// </summary>
        void MoveChild(object child, params double[] location);

        /// <summary>
        /// Removes the child.
        /// </summary>
        void RemoveChild(object child);
    }
}