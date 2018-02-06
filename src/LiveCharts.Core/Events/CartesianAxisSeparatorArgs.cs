using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Events
{
    /// <summary>
    /// The Cartesian Axis Separator Arguments.
    /// </summary>
    public class CartesianAxisSeparatorArgs
    {
        /// <summary>
        /// Gets or sets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public Rectangle Model { get; set; }

        /// <summary>
        /// Gets or sets the axis label model.
        /// </summary>
        /// <value>
        /// The axis label model.
        /// </value>
        public AxisLabelModel AxisLabelModel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is alternative.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is alternative; otherwise, <c>false</c>.
        /// </value>
        public bool IsAlternative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CartesianAxisSeparatorArgs"/> is disposing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposing; otherwise, <c>false</c>.
        /// </value>
        public bool Disposing { get; set; }

        /// <summary>
        /// Gets or sets the plane.
        /// </summary>
        /// <value>
        /// The plane.
        /// </value>
        public Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the chart view.
        /// </summary>
        /// <value>
        /// The chart view.
        /// </value>
        public IChartView ChartView { get; set; }
    }
}
