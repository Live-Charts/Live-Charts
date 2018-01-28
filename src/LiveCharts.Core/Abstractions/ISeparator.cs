using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Styles;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a separator view.
    /// </summary>
    public interface ISeparator : IResource
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        IPlaneLabelControl Label { get; }

        /// <summary>
        /// Moves the specified point1.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <param name="labelModel">The label model.</param>
        /// <param name="disposeWhenFinished">if set to <c>true</c> [dispose when finished].</param>
        /// <param name="style">The style.</param>
        /// <param name="plane">the sender plane.</param>
        /// <param name="chart">The chart.</param>
        void Move(Point point1, Point point2, AxisLabelModel labelModel, bool disposeWhenFinished, Style style, Plane plane, IChartView chart);
    }
}