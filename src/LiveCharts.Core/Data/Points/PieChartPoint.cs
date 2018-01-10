using LiveCharts.Core.Coordinates;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a pie chart point.
    /// </summary>
    /// <seealso cref="ChartPoint{T, U, V}" />
    public class PieChartPoint<TModel, TViewModel> : ChartPoint<TModel, StackedPoint, TViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartPoint{TModel, TViewModel}"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <param name="creationStamp">The creation stamp.</param>
        public PieChartPoint(int index, double value, object creationStamp)
            : base(creationStamp)
        {
            Coordinate = new StackedPoint(0d, value, index);
        }

        /// <inheritdoc cref="ChartPoint{T, U, X}.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            return false;
        }
    }
}