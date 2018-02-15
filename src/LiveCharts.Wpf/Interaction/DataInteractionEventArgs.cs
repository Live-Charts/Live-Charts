using System.Collections.Generic;
using System.Windows.Input;
using LiveCharts.Core.Data;

namespace LiveCharts.Wpf.Interaction
{
    /// <summary>
    /// User interaction with chart data event arguments.
    /// </summary>
    /// <seealso cref="System.Windows.Input.MouseButtonEventArgs" />
    public class DataInteractionEventArgs : MouseButtonEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataInteractionEventArgs"/> class.
        /// </summary>
        /// <param name="mouse">The mouse.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="button">The button.</param>
        /// <param name="points">The points.</param>
        public DataInteractionEventArgs(
            MouseDevice mouse, int timestamp, MouseButton button, IEnumerable<PackedPoint> points) 
            : base(mouse, timestamp, button)
        {
            Points = points;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInteractionEventArgs"/> class.
        /// </summary>
        /// <param name="mouse">The mouse.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="button">The button.</param>
        /// <param name="stylusDevice">The stylus device.</param>
        /// <param name="points">The points.</param>
        public DataInteractionEventArgs(
            MouseDevice mouse, int timestamp, MouseButton button, StylusDevice stylusDevice, IEnumerable<PackedPoint> points) 
            : base(mouse, timestamp, button, stylusDevice)
        {
            Points = points;
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public IEnumerable<PackedPoint> Points { get; }
    }
}
