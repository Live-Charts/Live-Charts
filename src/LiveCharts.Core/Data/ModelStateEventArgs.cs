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
        /// <param name="visual">The visual.</param>
        /// <param name="point">the point.</param>
        public ModelStateEventArgs(
            object visual,
            PackedPoint<TModel, TCoordinate> point)
        {
            Visual = visual;
            Point = point;
        }

        /// <summary>
        /// Gets a copy of the point in the chart.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public PackedPoint<TModel, TCoordinate> Point { get; }

        /// <summary>
        /// Gets the visual.
        /// </summary>
        /// <value>
        /// The visual.
        /// </value>
        public object Visual { get; }
    }
}